// <copyright file="SensoreControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            this.controller = fixture.Container.Resolve<SensorController>();
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

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnGetFramesAsync(device, start, stop);
                var objectResult = Assert.IsType<ObjectResult>(result);
                var response = Assert.IsAssignableFrom<Container<GetFrameResponse>>(objectResult.Value);

                Assert.NotNull(response);
            });
        }

        [Fact]
        public async Task OnGetFrames404NotFound()
        {
            var result = await this.controller.OnGetFramesAsync(Mocks.NotExisting.Id, null, null);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public void OnGetSensors200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Recording.Id,
                Mocks.Stopped.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnGetSensorsAsync(device);
                var objectResult = Assert.IsType<ObjectResult>(result);
                var response = Assert.IsAssignableFrom<Container<GetSensorResponse>>(objectResult.Value);

                Assert.NotNull(response);
            });
        }

        [Fact]
        public async Task OnGetSensors404NotFoundTest()
        {
            var result = await this.controller.OnGetSensorsAsync(Mocks.NotExisting.Id);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task OnGetSensorsAsync410GoneTest()
        {
            var result = await this.controller.OnGetSensorsAsync(Mocks.Archived.Id);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsArchived, response.Messages);
        }

        [Fact]
        public async Task OnGetSensorsAsync424FailedDependencyTest()
        {
            var result = await this.controller.OnGetSensorsAsync(Mocks.NotReachable.Id);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, response.Messages);
        }
    }
}
