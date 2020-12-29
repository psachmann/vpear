// <copyright file="DeviceControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Services;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Controllers
{
    public class DeviceControllerTest : IClassFixture<AutofacFixture>
    {
        private readonly DeviceController controller;

        public DeviceControllerTest(AutofacFixture fixture)
        {
            var logger = fixture.Container.Resolve<ILogger<DeviceController>>();
            var service = fixture.Container.Resolve<IDeviceService>();

            this.controller = new DeviceController(logger, service);
        }

        [Fact]
        public void GetAsync200OKTest()
        {
            var statuses = new List<DeviceStatus>()
            {
                DeviceStatus.Archived,
                DeviceStatus.NotReachable,
                DeviceStatus.Recording,
                DeviceStatus.Stopped,
            };

            statuses.ForEach(status =>
            {
                var result = this.controller.OnGet(status);
                var jsonResult = Assert.IsType<JsonResult>(result);
                var response = Assert.IsAssignableFrom<Container<GetDeviceResponse>>(jsonResult.Value);

                Assert.InRange(response.Count, 1, int.MaxValue);
            });
        }

        [Fact]
        public void GetAsync404NotFoundTest()
        {
            var result = this.controller.OnGet(DeviceStatus.None);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public void PutAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Stopped.Id,
                Mocks.Recording.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnPutAsync(device, new PutDeviceRequest());
                var jsonResult = Assert.IsType<JsonResult>(result);

                Assert.Null(jsonResult.Value);
            });
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotExisting.Id, new PutDeviceRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.Archived.Id, new PutDeviceRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsArchived, response.Messages);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotReachable.Id, new PutDeviceRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, response.Messages);
        }

        [Fact]
        public async Task PostAsync102ProcessingTest()
        {
            var request = new PostDeviceRequest()
            {
                StartIP = "0.0.0.0",
                StopIP = "255.255.255.255",
            };
            var result = await this.controller.OnPostAsync(request);
            var jsonResult = Assert.IsType<JsonResult>(result);

            Assert.Null(jsonResult.Value);
            Assert.Equal(StatusCodes.Status102Processing, jsonResult.StatusCode);
        }

        [Fact]
        public void DeleteAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.NotReachable.Id,
                Mocks.Stopped.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.controller.OnDeleteAsync(device);
                var jsonResult = Assert.IsType<JsonResult>(result);

                Assert.Null(jsonResult.Value);
            });
        }

        [Fact]
        public async Task DeleteAsync404NotFoundTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.NotExisting.Id);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task DeleteAsync409ConflictTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.Recording.Id);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsRecording, response.Messages);
        }
    }
}
