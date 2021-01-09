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
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Services;
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
        public void DeleteAsync409ConflictTest()
        {
            var devices = new List<Guid>()
            {
                Mocks.Archived.Id,
                Mocks.Recording.Id,
            };

            devices.ForEach(async device =>
            {
                var result = await this.service.DeleteAsync(device);

                Assert.NotNull(result.Error);
                Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
                Assert.Contains(ErrorMessages.DeviceIsArchivedOrRecording, result.Error!.Messages);
            });
        }

        [Theory]
        [InlineData(null, 60, null)]
        [InlineData(null, 60, 60)]
        [InlineData(300, 60, 300)]
        [InlineData(0, 60, 0)]
        [InlineData(1, 60, 1)]
        [InlineData(int.MinValue, 60, int.MinValue)]
        public async Task UpdateFrequencyAsyncTest(
            int? expectedFrequency,
            int currentFrequency,
            int? newFrquency)
        {
            var service = (DeviceService)this.service;
            var device = new Device()
            {
                Frequency = currentFrequency,
            };
            var result = await service.UpdateFrequencyAsync(device, newFrquency);

            Assert.Equal(expectedFrequency, result);
        }

        [Theory]
        [InlineData(60, 60)]
        [InlineData(1, 1)]
        [InlineData(1, 0)]
        [InlineData(1, int.MinValue)]
        [InlineData(int.MaxValue, int.MaxValue)]
        public void GetIntervallInSecondsTest(
            int expectedFrequency,
            int newFrquency)
        {
            var result = DeviceService.GetIntervallInSeconds(newFrquency);

            Assert.Equal(expectedFrequency, result);
        }

        [Theory]
        [InlineData(null, DeviceStatus.Archived, null)]
        [InlineData(null, DeviceStatus.NotReachable, null)]
        [InlineData(null, DeviceStatus.Recording, null)]
        [InlineData(null, DeviceStatus.Stopped, null)]
        [InlineData(null, DeviceStatus.Archived, DeviceStatus.Archived)]
        [InlineData(DeviceStatus.Archived, DeviceStatus.Archived, DeviceStatus.NotReachable)]
        [InlineData(DeviceStatus.Archived, DeviceStatus.Archived, DeviceStatus.Recording)]
        [InlineData(DeviceStatus.Archived, DeviceStatus.Archived, DeviceStatus.Stopped)]
        [InlineData(DeviceStatus.Archived, DeviceStatus.NotReachable, DeviceStatus.Archived)]
        [InlineData(null, DeviceStatus.NotReachable, DeviceStatus.NotReachable)]
        [InlineData(DeviceStatus.Recording, DeviceStatus.NotReachable, DeviceStatus.Recording)]
        [InlineData(DeviceStatus.Stopped, DeviceStatus.NotReachable, DeviceStatus.Stopped)]
        [InlineData(DeviceStatus.Archived, DeviceStatus.Recording, DeviceStatus.Archived)]
        [InlineData(DeviceStatus.NotReachable, DeviceStatus.Recording, DeviceStatus.NotReachable)]
        [InlineData(null, DeviceStatus.Recording, DeviceStatus.Recording)]
        [InlineData(DeviceStatus.Stopped, DeviceStatus.Recording, DeviceStatus.Stopped)]
        [InlineData(DeviceStatus.Archived, DeviceStatus.Stopped, DeviceStatus.Archived)]
        [InlineData(DeviceStatus.NotReachable, DeviceStatus.Stopped, DeviceStatus.NotReachable)]
        [InlineData(DeviceStatus.Recording, DeviceStatus.Stopped, DeviceStatus.Recording)]
        [InlineData(null, DeviceStatus.Stopped, DeviceStatus.Stopped)]
        public async Task UpdateStatusWithSuccessClientAsyncTest(
            DeviceStatus? expectedStatus,
            DeviceStatus currentStatus,
            DeviceStatus? newStatus)
        {
            var service = (DeviceService)this.service;
            var device = new Device()
            {
                Filter = new Filter(),
                Status = currentStatus,
            };
            var result = await service.UpdateStatusAsync(device, newStatus, Mocks.CreateSuccessDeviceClient());

            Assert.Equal(expectedStatus, result);
        }

        [Theory]
        [InlineData(DeviceStatus.Archived, DeviceStatus.NotReachable, DeviceStatus.Archived)]
        [InlineData(DeviceStatus.NotReachable, DeviceStatus.NotReachable, DeviceStatus.Stopped)]
        [InlineData(DeviceStatus.NotReachable, DeviceStatus.NotReachable, DeviceStatus.Recording)]
        public async Task UpdateStatusWithFailureClientAsyncTest(
            DeviceStatus? expectedStatus,
            DeviceStatus currentStatus,
            DeviceStatus? newStatus)
        {
            var service = (DeviceService)this.service;
            var device = new Device()
            {
                Filter = new Filter(),
                Status = currentStatus,
            };
            var result = await service.UpdateStatusAsync(device, newStatus, Mocks.CreateFailureDeviceClient());

            Assert.Equal(expectedStatus, result);
        }
    }
}
