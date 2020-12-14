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
    public class FiltersServiceTest
    {
        private readonly Guid existingDevice;
        private readonly Guid notExistingDevice;
        private readonly Guid archivedDevice;
        private readonly Guid notReachableDevice;
        private readonly IFiltersService service;

        public FiltersServiceTest()
        {
            this.existingDevice = DbSeed.Devices[1].Id;
            this.notExistingDevice = new Guid();
            this.archivedDevice = DbSeed.Devices[2].Id;
            this.notReachableDevice = DbSeed.Devices[3].Id;
            this.service = new FiltersService(Mocks.CreateLogger<FiltersController>(), Mocks.CreateRepository<Device, Guid>());
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
            var response = await this.service.PutAsync(this.existingDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync202AcceptedTest()
        {
            var response = await this.service.PutAsync(this.notReachableDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status202Accepted, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var response = await this.service.PutAsync(this.notExistingDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }
    }
}
