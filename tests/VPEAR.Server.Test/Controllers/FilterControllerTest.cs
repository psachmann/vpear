using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Controllers
{
    public class FilterControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public FilterControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData(true, HttpStatusCode.OK, "00000000-0000-0000-0000-000000000001")]
        [InlineData(false, HttpStatusCode.BadRequest, null)]
        [InlineData(false, HttpStatusCode.BadRequest, "")]
        [InlineData(false, HttpStatusCode.Unauthorized, "00000000-0000-0000-0000-000000000001")]
        [InlineData(false, HttpStatusCode.NotFound, "00000000-0000-0000-0000-000000000010")]
        public async Task OnGetAsyncTest(
            bool checkContent,
            HttpStatusCode expectedStatus,
            string id)
        {
            var client = this.factory.CreateClient();
            // client.DefaultRequestHeaders.Add("Authorization: Bearer", token);

            var response = await client.GetAsync($"{Routes.FiltersRoute}?id={id}");

            if (checkContent)
            {
                Assert.NotEmpty(await response.Content.ReadAsStringAsync());
            }
            else
            {
                Assert.Empty(await response.Content.ReadAsStringAsync());
            }

            Assert.Equal(expectedStatus, response.StatusCode);
        }

        public async Task OnPutAsync()
        {

        }
    }
}
