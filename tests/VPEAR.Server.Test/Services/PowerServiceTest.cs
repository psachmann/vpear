using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Controllers;
using VPEAR.Server.Db;
using VPEAR.Server.Services;
using Xunit;

namespace VPEAR.Server.Test
{
    [Collection("PowerServiceTest")]
    public class PowerServiceTest
    {
        private readonly Guid stoppedDevice = DbSeed.Devices[0].Id;
        private readonly Guid recordingDevice = DbSeed.Devices[1].Id;
        private readonly Guid archivedDevice = DbSeed.Devices[2].Id;
        private readonly Guid notReachableDevice = DbSeed.Devices[3].Id;
        private readonly Guid notExistingDevice = new Guid();
        private readonly VPEARDbContext context;
        private readonly IPowerService service;

        public PowerServiceTest()
        {
            this.context = Mocks.CreateDbContext();
            this.service = new PowerService(
                Mocks.CreateLogger<PowerController>(),
                Mocks.CreateRepository<Device, Guid>(this.context),
                Mocks.CreateDeviceClientFactory());
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var devices = new List<Guid>()
            {
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

        [Fact]
        public async Task GetAsync410GoneTest()
        {
            var response = await this.service.GetAsync(this.archivedDevice);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
        }

        [Fact]
        public async Task GetAsync424FailedDependencyTest()
        {
            var response = await this.service.GetAsync(this.notReachableDevice);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
        }
    }
}
