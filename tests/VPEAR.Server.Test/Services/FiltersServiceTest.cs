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
    public class FiltersServiceTest : IDisposable
    {
        private readonly Guid stoppedDevice = DbSeed.Devices[0].Id;
        private readonly Guid recordingDevice = DbSeed.Devices[1].Id;
        private readonly Guid archivedDevice = DbSeed.Devices[2].Id;
        private readonly Guid notReachableDevice = DbSeed.Devices[3].Id;
        private readonly Guid notExistingDevice = new Guid();
        private readonly VPEARDbContext context;
        private readonly IFiltersService service;

        public FiltersServiceTest()
        {
            this.context = Mocks.CreateDbContext();
            this.service = new FiltersService(
                Mocks.CreateLogger<FiltersController>(),
                Mocks.CreateRepository<Device, Guid>(this.context),
                Mocks.CreateRepository<Filters, Guid>(this.context));
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var response = await this.service.GetAsync(this.stoppedDevice);

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
            var response = await this.service.PutAsync(this.recordingDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync202AcceptedTest()
        {
            var response = await this.service.PutAsync(this.stoppedDevice, new PutFiltersRequest());

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

        [Fact]
        public async Task PutAsync410FailedDependencyTest()
        {
            var response = await this.service.PutAsync(this.archivedDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status410Gone, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync424FailedDependencyTest()
        {
            var response = await this.service.PutAsync(this.notReachableDevice, new PutFiltersRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status424FailedDependency, response.StatusCode);
        }
    }
}
