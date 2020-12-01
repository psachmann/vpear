using System;
using System.Net;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Services;
using Xunit;

namespace VPEAR.Server.Test
{
    public class FiltersServiceTest : IClassFixture<FiltersService>
    {
        private readonly IFiltersService filtersService;

        public FiltersServiceTest(FiltersService filtersService)
        {
            this.filtersService = filtersService;
        }

        [Fact]
        public async void GetAsync200OKTest()
        {
            var id = new Guid("00000000-0000-0000-0000-000000000001");
            var response = await this.filtersService.GetAsync(id);

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Payload);
        }

        [Fact]
        public async void GetAsync404NotFoundTest()
        {
            var id = new Guid();
            var response = await this.filtersService.GetAsync(id);

            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.Null(response.Payload);
        }

        [Fact]
        public async void PutAsync200OKTest()
        {
            var id = new Guid("00000000-0000-0000-0000-000000000001");
            var request = new PutFiltersRequest()
            {
                Noise = true,
                Smooth = true,
                Spot = true,
            };
            var response = await this.filtersService.PutAsync(id, request);

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.Null(response.Payload);
        }

        [Fact]
        public async void PutAsync202AcceptedTest()
        {
            var id = new Guid("00000000-0000-0000-0000-000000000002");
            var request = new PutFiltersRequest()
            {
                Noise = true,
                Smooth = true,
                Spot = true,
            };
            var response = await this.filtersService.PutAsync(id, request);

            Assert.Equal((int)HttpStatusCode.Accepted, response.StatusCode);
            Assert.Null(response.Payload);
        }

        [Fact]
        public async void PutAsync404NotFoundTest()
        {
            var id = new Guid();
            var request = new PutFiltersRequest()
            {
                Noise = true,
                Smooth = true,
                Spot = true,
            };
            var response = await this.filtersService.PutAsync(id, request);

            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.Null(response.Payload);
        }
    }
}
