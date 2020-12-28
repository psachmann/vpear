// <copyright file="PowerControllerTest.cs" company="Patrick Sachmann">
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
    public class PowerControllerTest : IClassFixture<AutofacFixture>
    {
        private readonly PowerController controller;

        public PowerControllerTest(AutofacFixture fixture)
        {
            var logger = fixture.Container.Resolve<ILogger<PowerController>>();
            var mock = new Mock<IPowerService>();

            mock.Setup(mock => mock.GetAsync(Mocks.Archived.Id))
                .ReturnsAsync(new Result<Core.Wrappers.GetPowerResponse>(
                    HttpStatusCode.Gone,
                    ErrorMessages.DeviceIsArchived));

            mock.Setup(mock => mock.GetAsync(Mocks.NotExisting.Id))
                .ReturnsAsync(new Result<Core.Wrappers.GetPowerResponse>(
                    HttpStatusCode.NotFound,
                    ErrorMessages.DeviceNotFound));

            mock.Setup(mock => mock.GetAsync(Mocks.NotReachable.Id))
                .ReturnsAsync(new Result<Core.Wrappers.GetPowerResponse>(
                    HttpStatusCode.FailedDependency,
                    ErrorMessages.DeviceIsNotReachable));

            mock.Setup(mock => mock.GetAsync(Mocks.Recording.Id))
                .ReturnsAsync(new Result<Core.Wrappers.GetPowerResponse>(
                    HttpStatusCode.OK,
                    new GetPowerResponse()));

            mock.Setup(mock => mock.GetAsync(Mocks.Stopped.Id))
                .ReturnsAsync(new Result<Core.Wrappers.GetPowerResponse>(
                    HttpStatusCode.OK,
                    new GetPowerResponse()));

            this.controller = new PowerController(logger, mock.Object);
        }

        [Fact]
        public void OnGetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Recording.Id,
                Mocks.Stopped.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnGetAsync(device);
                var jsonResult = Assert.IsType<JsonResult>(result);
                var response = Assert.IsAssignableFrom<GetPowerResponse>(jsonResult.Value);

                Assert.NotNull(response);
            });
        }

        [Fact]
        public async Task OnGetAsync404NotFoundTest()
        {
            var result = await this.controller.OnGetAsync(Mocks.NotExisting.Id);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task OnGetAsync410GoneTest()
        {
            var result = await this.controller.OnGetAsync(Mocks.Archived.Id);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsArchived, response.Messages);
        }

        [Fact]
        public async Task OnGetAsync424FailedDependencyTest()
        {
            var result = await this.controller.OnGetAsync(Mocks.NotReachable.Id);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, response.Messages);
        }
    }
}
