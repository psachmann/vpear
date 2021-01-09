// <copyright file="AdminIntegrationTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Extensions;
using VPEAR.Core.Wrappers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Integration
{
    [Collection("IntegrationTest")]
    public class AdminIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string BaseAddress = "http://localhost";
        private readonly WebApplicationFactory<Startup> factory;

        public AdminIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task LoginAsyncSuccessTest()
        {
            var client = new VPEARClient(BaseAddress, this.factory.CreateClient());
            var result = await client.LoginAsync(Defaults.DefaultAdminName, Defaults.DefaultAdminPassword);

            Assert.True(result, "Login should be successful.");
            Assert.Equal(HttpStatusCode.OK, client.Response.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, null, null)]
        [InlineData(HttpStatusCode.BadRequest, "", "")]
        [InlineData(HttpStatusCode.NotFound, "user", "password")]
        public async Task LoginAsyncFailureTest(
            HttpStatusCode expectedStatus,
            string? name,
            string? password)
        {
            var client = new VPEARClient(BaseAddress, this.factory.CreateClient());
            var result = await client.LoginAsync(name, password);

            Assert.False(result, "Login should NOT be successful.");
            Assert.Equal(expectedStatus, client.Response.StatusCode);
        }
    }
}
