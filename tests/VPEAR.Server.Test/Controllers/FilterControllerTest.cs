// <copyright file="FilterControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FilterControllerTest
    {
        private readonly Guid archivedDevice = new Guid("00000000000000000000000000000001");
        private readonly Guid notExistingDevice = new Guid("00000000000000000000000000000000");
        private readonly Guid notReachableDevice = new Guid("00000000000000000000000000000002");
        private readonly Guid recordingDevice = new Guid("00000000000000000000000000000003");
        private readonly Guid stoppedDevice = new Guid("00000000000000000000000000000004");
        private readonly FilterController controller;

        public FilterControllerTest()
        {
            var logger = Mocks.CreateLogger<FilterController>();
            var mock = new Mock<IFilterService>();

            mock.Setup(service => service.GetAsync(this.archivedDevice))
                .ReturnsAsync(new Result<GetFiltersResponse>(
                    HttpStatusCode.OK,
                    new GetFiltersResponse()));

            mock.Setup(service => service.GetAsync(this.notReachableDevice))
                .ReturnsAsync(new Result<GetFiltersResponse>(
                    HttpStatusCode.OK,
                    new GetFiltersResponse()));

            mock.Setup(service => service.GetAsync(this.recordingDevice))
                .ReturnsAsync(new Result<GetFiltersResponse>(
                    HttpStatusCode.OK,
                    new GetFiltersResponse()));

            mock.Setup(service => service.GetAsync(this.stoppedDevice))
                .ReturnsAsync(new Result<GetFiltersResponse>(
                    HttpStatusCode.OK,
                    new GetFiltersResponse()));

            mock.Setup(service => service.GetAsync(this.notExistingDevice))
                .ReturnsAsync(new Result<GetFiltersResponse>(
                    HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound));

            mock.Setup(service => service.PutAsync(this.archivedDevice, It.IsAny<PutFiltersRequest>()))
                .ReturnsAsync(new Result<Null>(
                    HttpStatusCode.Gone, ErrorMessages.DeviceIsArchived));

            mock.Setup(service => service.PutAsync(this.notExistingDevice, It.IsAny<PutFiltersRequest>()))
                .ReturnsAsync(new Result<Null>(
                    HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound));

            mock.Setup(service => service.PutAsync(this.notReachableDevice, It.IsAny<PutFiltersRequest>()))
                .ReturnsAsync(new Result<Null>(
                    HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable));

            mock.Setup(service => service.PutAsync(this.recordingDevice, It.IsAny<PutFiltersRequest>()))
                .ReturnsAsync(new Result<Null>(statusCode: HttpStatusCode.OK, value: null));

            mock.Setup(service => service.PutAsync(this.stoppedDevice, It.IsAny<PutFiltersRequest>()))
                .ReturnsAsync(new Result<Null>(statusCode: HttpStatusCode.OK, value: null));

            this.controller = new FilterController(logger, mock.Object);
        }

        [Fact]
        public void OnGetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                this.archivedDevice,
                this.notReachableDevice,
                this.recordingDevice,
                this.stoppedDevice,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnGetAsync(device);
                var jsonResult = Assert.IsType<JsonResult>(result);
                var response = Assert.IsAssignableFrom<GetFiltersResponse>(jsonResult.Value);

                Assert.NotNull(response);
            });
        }

        [Fact]
        public async Task OnGetAsync404NotFound()
        {
            var result = await this.controller.OnGetAsync(this.notExistingDevice);
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
                this.recordingDevice,
                this.stoppedDevice,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnPutAsync(device, new PutFiltersRequest());
                var jsonResult = Assert.IsType<JsonResult>(result);

                Assert.Null(jsonResult.Value);
            });
        }

        [Fact]
        public async Task OnPutAsync404NotFoundTest()
        {
            var result = await this.controller.OnPutAsync(this.notExistingDevice, new PutFiltersRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task OnPutAsync410GoneTest()
        {
            var result = await this.controller.OnPutAsync(this.archivedDevice, new PutFiltersRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsArchived, response.Messages);
        }

        [Fact]
        public async Task OnPutAsync424FailedDependencyTest()
        {
            var result = await this.controller.OnPutAsync(this.notReachableDevice, new PutFiltersRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, response.Messages);
        }
    }
}
