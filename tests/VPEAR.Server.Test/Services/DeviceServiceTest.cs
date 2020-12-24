// <copyright file="DeviceServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

/*
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Db;
using VPEAR.Server.Services;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class DeviceServiceTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly Guid stoppedDevice = DbSeed.Devices[0].Id;
        private readonly Guid recordingDevice = DbSeed.Devices[1].Id;
        private readonly Guid archivedDevice = DbSeed.Devices[2].Id;
        private readonly Guid notReachableDevice = DbSeed.Devices[3].Id;
        private readonly Guid notExistingDevice = new Guid();
        private readonly IDeviceService service;

        public DeviceServiceTest(VPEARDbContextFixture fixture)
        {
            this.service = new DeviceService(
                Mocks.CreateLogger<DeviceController>(),
                Mocks.CreateRepository<Device, Guid>(fixture.Context));
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var response = await this.service.GetAsync(DeviceStatus.Archived);

            Assert.NotNull(response.Payload);

            var devices = Assert.IsAssignableFrom<Container<GetDeviceResponse>>(response.Payload);

            Assert.InRange(devices.Count, 1, int.MaxValue);
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var response = await this.service.GetAsync((DeviceStatus)5);

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);

            var error = Assert.IsAssignableFrom<ErrorResponse>(response.Payload);

            Assert.Equal(StatusCodes.Status404NotFound, error.StatusCode);
            Assert.Equal(ErrorMessages.DeviceNotFound, error.Message);
        }

        [Fact]
        public async Task PutAsync200OKTest()
        {
            var response = await this.service.PutAsync(this.stoppedDevice, new PutDeviceRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var response = await this.service.PutAsync(this.notExistingDevice, new PutDeviceRequest());

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);

            var error = Assert.IsAssignableFrom<ErrorResponse>(response.Payload);

            Assert.Equal(StatusCodes.Status404NotFound, error.StatusCode);
            Assert.Equal(ErrorMessages.DeviceNotFound, error.Message);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var devices = (await this.service.GetAsync(DeviceStatus.Archived))
                .Payload!.Items;

            Assert.NotEmpty(devices);

            var response = await this.service.PutAsync(devices.First(), new PutDeviceRequest());

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);

            var error = Assert.IsAssignableFrom<ErrorResponse>(response.Payload);

            Assert.Equal(StatusCodes.Status410Gone, error.StatusCode);
            Assert.Equal(ErrorMessages.DeviceIsArchived, error.Message);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var response = await this.service.PutAsync(this.notReachableDevice, new PutDeviceRequest());

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);

            var error = Assert.IsAssignableFrom<ErrorResponse>(response.Payload);

            Assert.Equal(StatusCodes.Status424FailedDependency, error.StatusCode);
            Assert.Equal(ErrorMessages.DeviceIsNotReachable, error.Message);
        }

        [Fact]
        public async Task PostAsync200OKTest()
        {
            var request = new PostDeviceRequest()
            {
                StartIP = "0.0.0.0",
                StopIP = "255.255.255.255",
            };
            var response = await this.service.PostAsync(request);

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync404Test()
        {
            var request = new PostDeviceRequest()
            {
                StartIP = "0.0.0.0",
                StopIP = "0.0.0.0",
            };
            var response = await this.service.PostAsync(request);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync200OKTest()
        {
            var response = await this.service.DeleteAsync(this.archivedDevice);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync404NotFoundTest()
        {
            var response = await this.service.DeleteAsync(this.notExistingDevice);

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync409ConflictTest()
        {
            var response = await this.service.DeleteAsync(this.recordingDevice);

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
        }
    }
}
*/
