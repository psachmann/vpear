using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public class PowerControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public PowerControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void OnGetAsync200OKTest()
        {
            var client = this.factory.CreateClient();
            var respone = await client.GetAsync($"{Routes.PowerRoute}?id=00000000-0000-0000-0000-000000000001");

            Assert.Equal(HttpStatusCode.OK, respone.StatusCode);
            Assert.NotEmpty(await respone.Content.ReadAsStringAsync());
        }

        [Fact]
        public async void OnGetAsync202AcceptedTest()
        {
            var client = this.factory.CreateClient();
            var respone = await client.GetAsync($"{Routes.PowerRoute}?id=00000000-0000-0000-0000-000000000002");

            Assert.Equal(HttpStatusCode.Accepted, respone.StatusCode);
            Assert.Empty(await respone.Content.ReadAsStringAsync());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("fdasdf")]
        [InlineData("00000000-0000-0000-/)(/-000000000002")]
        public async void OnGetAsync400BadRequest(string? id)
        {
            var client = this.factory.CreateClient();
            var respone = await client.GetAsync($"{Routes.PowerRoute}?id={id}");

            Assert.Equal(HttpStatusCode.BadRequest, respone.StatusCode);
            Assert.Empty(await respone.Content.ReadAsStringAsync());
        }

        [Fact]
        public async void OnGetAsync401Unauthorized()
        {
            var client = this.factory.CreateClient();
            var respone = await client.GetAsync($"{Routes.PowerRoute}?id=00000000-0000-0000-0000-000000000001");

            Assert.Equal(HttpStatusCode.Unauthorized, respone.StatusCode);
            Assert.Empty(await respone.Content.ReadAsStringAsync());
        }
    }
}
