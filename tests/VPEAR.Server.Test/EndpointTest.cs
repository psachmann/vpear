using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    [Collection("EndpointTest")]
    public class EndpointTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Get = "GET";
        private const string Put = "PUT";
        private const string Post = "POST";
        private const string Delete = "DELETE";
        private readonly WebApplicationFactory<Startup> factory;

        public EndpointTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData(Routes.BaseRoute, Get)]
        [InlineData(Routes.DeviceRoute, Get)]
        [InlineData(Routes.DeviceRoute, Put)]
        [InlineData(Routes.DeviceRoute, Post)]
        [InlineData(Routes.DeviceRoute, Delete)]
        [InlineData(Routes.SensorsRoute, Get)]
        [InlineData(Routes.FramesRoute, Get)]
        [InlineData(Routes.FiltersRoute, Get)]
        [InlineData(Routes.FiltersRoute, Put)]
        [InlineData(Routes.PowerRoute, Get)]
        [InlineData(Routes.WifiRoute, Get)]
        [InlineData(Routes.WifiRoute, Put)]
        [InlineData(Routes.FirmwareRoute, Get)]
        [InlineData(Routes.FirmwareRoute, Put)]
        public async Task EndpointExistenceTest(string url, string method)
        {
            var client = this.factory.CreateClient();
            var response = method switch
            {
                Get => await client.GetAsync(url),
                Put => await client.PutAsync(url, new StringContent("{}")),
                Post => await client.PostAsync(url, new StringContent("{}")),
                Delete => await client.DeleteAsync(url),
                _ => throw new ArgumentOutOfRangeException(nameof(method)),
            };

            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
