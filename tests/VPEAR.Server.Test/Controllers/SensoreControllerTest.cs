// <copyright file="SensoreControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Controllers
{
    public class SensoreControllerTest : IClassFixture<AutofacFixture>
    {
        private readonly SensorController controller;

        public SensoreControllerTest(AutofacFixture fixture)
        {
            var logger = fixture.Container.Resolve<ILogger<SensorController>>();
            var service = fixture.Container.Resolve<ISensorService>();
            var mock = new Mock<ISensorService>();

            mock.Setup(mock => mock.GetFrames(Mocks.Archived.Id, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Result<Container<GetFrameResponse>>(
                    HttpStatusCode.OK,
                    new Container<GetFrameResponse>()));

            mock.Setup(mock => mock.GetFrames(Mocks.NotReachable.Id, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Result<Container<GetFrameResponse>>(
                    HttpStatusCode.OK,
                    new Container<GetFrameResponse>()));

            mock.Setup(mock => mock.GetFrames(Mocks.Recording.Id, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Result<Container<GetFrameResponse>>(
                    HttpStatusCode.OK,
                    new Container<GetFrameResponse>()));

            mock.Setup(mock => mock.GetFrames(Mocks.Stopped.Id, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Result<Container<GetFrameResponse>>(
                    HttpStatusCode.OK,
                    new Container<GetFrameResponse>()));

            mock.Setup(mock => mock.GetFrames(Mocks.NotExisting.Id, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Result<Container<GetFrameResponse>>(
                    HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound));

            mock.Setup(mock => mock.GetSensors(Mocks.Archived.Id))
                .Returns(new Result<Container<GetSensorResponse>>(
                    HttpStatusCode.OK,
                    new Container<GetSensorResponse>()));

            mock.Setup(mock => mock.GetSensors(Mocks.NotReachable.Id))
                .Returns(new Result<Container<GetSensorResponse>>(
                    HttpStatusCode.OK,
                    new Container<GetSensorResponse>()));

            mock.Setup(mock => mock.GetSensors(Mocks.Recording.Id))
                .Returns(new Result<Container<GetSensorResponse>>(
                    HttpStatusCode.OK,
                    new Container<GetSensorResponse>()));

            mock.Setup(mock => mock.GetSensors(Mocks.Stopped.Id))
                .Returns(new Result<Container<GetSensorResponse>>(
                    HttpStatusCode.OK,
                    new Container<GetSensorResponse>()));

            mock.Setup(mock => mock.GetSensors(Mocks.NotExisting.Id))
                .Returns(new Result<Container<GetSensorResponse>>(
                    HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound));

            this.controller = new SensorController(logger, service);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, int.MaxValue)]
        public void OnGetFrames200OKTest(int start, int stop)
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.NotReachable.Id,
                Mocks.Recording.Id,
                Mocks.Stopped.Id,
            };

            devices.ForEach(device =>
            {
                var result = this.controller.OnGetFrames(device, start, stop);
                var jsonResult = Assert.IsType<JsonResult>(result);
                var response = Assert.IsAssignableFrom<Container<GetFrameResponse>>(jsonResult.Value);

                Assert.NotNull(response);
            });
        }

        [Fact]
        public void OnGetFrames404NotFound()
        {
            var result = this.controller.OnGetFrames(Mocks.NotExisting.Id, null, null);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public void OnGetSensors200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.NotReachable.Id,
                Mocks.Recording.Id,
                Mocks.Stopped.Id,
            };

            devices.ForEach(device =>
            {
                var result = this.controller.OnGetSensors(device);
                var jsonResult = Assert.IsType<JsonResult>(result);
                var response = Assert.IsAssignableFrom<Container<GetSensorResponse>>(jsonResult.Value);

                Assert.NotNull(response);
            });
        }

        [Fact]
        public void OnGetSensors404NotFound()
        {
            var result = this.controller.OnGetSensors(Mocks.NotExisting.Id);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }
    }
}
