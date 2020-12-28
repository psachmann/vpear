// <copyright file="WifiControllerTest.cs" company="Patrick Sachmann">
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
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Controllers
{
    public class WifiControllerTest : IClassFixture<AutofacFixture>
    {
        private readonly WifiController controller;

        public WifiControllerTest(AutofacFixture fixture)
        {
            var logger = fixture.Container.Resolve<ILogger<WifiController>>();
            var mock = new Mock<IWifiService>();

            mock.Setup(mock => mock.Get(Mocks.Archived.Id))
                .Returns(new Result<GetWifiResponse>(
                    HttpStatusCode.OK, new GetWifiResponse()));

            mock.Setup(mock => mock.Get(Mocks.NotExisting.Id))
                .Returns(new Result<GetWifiResponse>(
                    HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound));

            mock.Setup(mock => mock.Get(Mocks.NotReachable.Id))
                .Returns(new Result<GetWifiResponse>(
                    HttpStatusCode.OK, new GetWifiResponse()));

            mock.Setup(mock => mock.Get(Mocks.Recording.Id))
                .Returns(new Result<GetWifiResponse>(
                    HttpStatusCode.OK, new GetWifiResponse()));

            mock.Setup(mock => mock.Get(Mocks.Stopped.Id))
                .Returns(new Result<GetWifiResponse>(
                    HttpStatusCode.OK, new GetWifiResponse()));

            mock.Setup(mock => mock.PutAsync(Mocks.Archived.Id, It.IsAny<PutWifiRequest>()))
                .ReturnsAsync(new Result<Null>(HttpStatusCode.Gone, ErrorMessages.DeviceIsArchived));

            mock.Setup(mock => mock.PutAsync(Mocks.NotExisting.Id, It.IsAny<PutWifiRequest>()))
                .ReturnsAsync(new Result<Null>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound));

            mock.Setup(mock => mock.PutAsync(Mocks.NotReachable.Id, It.IsAny<PutWifiRequest>()))
                .ReturnsAsync(new Result<Null>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable));

            mock.Setup(mock => mock.PutAsync(Mocks.Recording.Id, It.IsAny<PutWifiRequest>()))
                .ReturnsAsync(new Result<Null>(HttpStatusCode.OK));

            mock.Setup(mock => mock.PutAsync(Mocks.Stopped.Id, It.IsAny<PutWifiRequest>()))
                .ReturnsAsync(new Result<Null>(HttpStatusCode.OK));

            this.controller = new WifiController(logger, mock.Object);
        }

        [Fact]
        public void OnGet200OKTest()
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
                var result = this.controller.OnGet(device);
                var jsonResult = Assert.IsType<JsonResult>(result);
                var response = Assert.IsAssignableFrom<GetWifiResponse>(jsonResult.Value);

                Assert.NotNull(response);
            });
        }

        [Fact]
        public void OnGet404NOtFoundTest()
        {
            var result = this.controller.OnGet(Mocks.NotExisting.Id);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public void OnPutAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Recording.Id,
                Mocks.Stopped.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnPutAsync(device, new PutWifiRequest());
                var jsonResult = Assert.IsType<JsonResult>(result);

                Assert.Null(jsonResult.Value);
            });
        }

        [Fact]
        public async Task OnPutAsync404NotFoundTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotExisting.Id, new PutWifiRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task OnPutAsync410GoneTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.Archived.Id, new PutWifiRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsArchived, response.Messages);
        }

        [Fact]
        public async Task OnPutAsync424FailedDependencyTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotReachable.Id, new PutWifiRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, response.Messages);
        }
    }
}
