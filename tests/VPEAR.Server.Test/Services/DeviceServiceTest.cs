// <copyright file="DeviceServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class DeviceServiceTest : IClassFixture<AutofacFixture>
    {
        private readonly IDeviceService service;

        public DeviceServiceTest(AutofacFixture fixture)
        {
            this.service = fixture.Container.Resolve<IDeviceService>();
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
                var result = await this.service.GetAsync(status);

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.NotNull(result.Value);
                Assert.InRange(result.Value!.Count, 1, int.MaxValue);
            });
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var result = await this.service.GetAsync(DeviceStatus.None);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
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
                var result = await this.service.PutAsync(device, new PutDeviceRequest());

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Null(result.Value);
            });
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.service.PutAsync(Mocks.NotExisting.Id, new PutDeviceRequest());

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var result = await this.service.PutAsync(Mocks.Archived.Id, new PutDeviceRequest());

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status410Gone, result.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsArchived, result.Error!.Messages);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var result = await this.service.PutAsync(Mocks.NotReachable.Id, new PutDeviceRequest());

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status424FailedDependency, result.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsNotReachable, result.Error!.Messages);
        }

        [Fact]
        public async Task PostAsync102ProcessingTest()
        {
            var request = new PostDeviceRequest()
            {
                Address = "192.168.178.33",
                SubnetMask = "255.255.255.0",
            };
            var result = await this.service.PostAsync(request);

            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status102Processing, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync400BadRequestTest()
        {
            var request = new PostDeviceRequest()
            {
                Address = "192.168.178.33",
                SubnetMask = "255::0",
            };
            var result = await this.service.PostAsync(request);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Contains(ErrorMessages.BadRequest, result.Error!.Messages);
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
                var result = await this.service.DeleteAsync(device);

                Assert.Null(result.Value);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            });
        }

        [Fact]
        public async Task DeleteAsync404NotFoundTest()
        {
            var result = await this.service.DeleteAsync(Mocks.NotExisting.Id);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.DeviceNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task DeleteAsync409ConflictTest()
        {
            var result = await this.service.DeleteAsync(Mocks.Recording.Id);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
            Assert.Contains(ErrorMessages.DeviceIsRecording, result.Error!.Messages);
        }
    }
}
