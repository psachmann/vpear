using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Db;
using VPEAR.Server.Services;
using Xunit;

namespace VPEAR.Server.Test
{
    public class FirmwareServiceTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly Guid stoppedDevice = DbSeed.Devices[0].Id;
        private readonly Guid recordingDevice = DbSeed.Devices[1].Id;
        private readonly Guid archivedDevice = DbSeed.Devices[2].Id;
        private readonly Guid notReachableDevice = DbSeed.Devices[3].Id;
        private readonly Guid notExistingDevice = new Guid();
        private readonly IFirmwareService service;

        public FirmwareServiceTest(VPEARDbContextFixture fixture)
        {
            this.service = new FirmwareService(
                Mocks.CreateLogger<FirmwareController>(),
                Mocks.CreateRepository<Device, Guid>(fixture.Context));
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
                this.archivedDevice,
                this.notReachableDevice,
                this.stoppedDevice,
                this.recordingDevice,
            };

            foreach (var device in devices)
            {
                var response = await this.service.GetAsync(device);

                Assert.NotNull(response.Payload);
                Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var response = await this.service.GetAsync(this.notExistingDevice);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, false, "stable", null)]
        [InlineData(StatusCodes.Status200OK, false, "unstable", null)]
        [InlineData(StatusCodes.Status200OK, false, "stable", "next")]
        [InlineData(StatusCodes.Status200OK, false, "unstable", "next")]
        [InlineData(StatusCodes.Status202Accepted, true, null, null)]
        public async Task PutAsync2XXTest(
            int expectedStatus,
            bool package,
            string? source,
            string? upgrade)
        {
            var request = new PutFirmwareRequest()
            {
                Package = package,
                Source = source,
                Upgrade = upgrade,
            };
            var response = await this.service.PutAsync(this.stoppedDevice, request);

            Assert.Null(response.Payload);
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var response = await this.service.PutAsync(this.notExistingDevice, new PutFirmwareRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var response = await this.service.PutAsync(this.archivedDevice, new PutFirmwareRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var response = await this.service.PutAsync(this.notReachableDevice, new PutFirmwareRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
        }
    }
}
