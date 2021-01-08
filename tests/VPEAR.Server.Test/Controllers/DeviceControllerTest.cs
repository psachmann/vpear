// <copyright file="DeviceControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Controllers
{
    public class DeviceControllerTest : IClassFixture<AutofacFixture>
    {
        private readonly DeviceController controller;

        public DeviceControllerTest(AutofacFixture fixture)
        {
            this.controller = fixture.Container.Resolve<DeviceController>();
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

            statuses.ForEach(async status =>
            {
                var result = await this.controller.OnGetAsync(status);
                var objectResult = Assert.IsType<ObjectResult>(result);
                var response = Assert.IsAssignableFrom<Container<GetDeviceResponse>>(objectResult.Value);

                Assert.InRange(response.Count, 1, int.MaxValue);
            });
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var result = await this.controller.OnGetAsync(DeviceStatus.None);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

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
                var objectResult = Assert.IsType<ObjectResult>(result);

                Assert.Null(objectResult.Value);
            });
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotExisting.Id, new PutDeviceRequest());
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.Archived.Id, new PutDeviceRequest());
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsArchived, response.Messages);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotReachable.Id, new PutDeviceRequest());
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, response.Messages);
        }

        [Fact]
        public async Task PostAsync102ProcessingTest()
        {
            var request = new PostDeviceRequest()
            {
                Address = "192.168.178.33",
                SubnetMask = "255.255.255.0",
            };
            var result = await this.controller.OnPostAsync(request);
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async Task PostAsync400BadRequestTest()
        {
            var request = new PostDeviceRequest()
            {
                Address = "192.168.178.33",
                SubnetMask = "255::0",
            };
            var result = await this.controller.OnPostAsync(request);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
            Assert.Contains(ErrorMessages.BadRequest, response.Messages);
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
                var objectResult = Assert.IsType<ObjectResult>(result);

                Assert.Null(objectResult.Value);
            });
        }

        [Fact]
        public async Task DeleteAsync404NotFoundTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.NotExisting.Id);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, response.Messages);
        }

        [Fact]
        public async Task DeleteAsync409ConflictTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.Recording.Id);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsRecording, response.Messages);
        }
    }
}
