using Microsoft.AspNetCore.Http;
using System;
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
    public class WifiServiceTest
    {
        private readonly Guid existingDevice;
        private readonly Guid notExistingDevice;
        private readonly Guid archivedDevice;
        private readonly Guid notReachableDevice;
        private readonly IWifiService service;

        public WifiServiceTest()
        {
            this.existingDevice = DbSeed.Devices[1].Id;
            this.notExistingDevice = new Guid();
            this.archivedDevice = DbSeed.Devices[2].Id;
            this.notReachableDevice = DbSeed.Devices[3].Id;
            this.service = new WifiService(Mocks.CreateLogger<WifiController>(), Mocks.CreateRepository<Device, Guid>());
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var response = await this.service.GetAsync(this.existingDevice);

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var response = await this.service.GetAsync(this.notExistingDevice);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync200OKTest()
        {
            var response = await this.service.PutAsync(this.existingDevice, new PutWifiRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync404NoFoundTest()
        {
            var response = await this.service.PutAsync(this.notExistingDevice, new PutWifiRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync410GoneTest()
        {
            var response = await this.service.PutAsync(this.archivedDevice, new PutWifiRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var response = await this.service.PutAsync(this.notReachableDevice, new PutWifiRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
        }
    }
}
