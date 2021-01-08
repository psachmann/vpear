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
    public class AdminIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly VPEARClient client;

        public AdminIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.client = new VPEARClient("http://localhost", factory.CreateClient());
        }

        [Fact]
        [Priority(0)]
        public async Task LoginAsyncScuccessTest()
        {
            var result = await this.client.LoginAsync("admin", "password");

            Assert.True(result, "Login should be successful.");
            Assert.Equal(HttpStatusCode.OK, this.client.Response.StatusCode);
        }

        [Theory]
        [Priority(-1)]
        [InlineData(HttpStatusCode.BadRequest, null, null)]
        [InlineData(HttpStatusCode.BadRequest, "", "")]
        [InlineData(HttpStatusCode.NotFound, "user", "password")]
        public async Task LoginAsyncFailureTest(
            HttpStatusCode expectedStatus,
            string? name,
            string? password)
        {
            var result = await this.client.LoginAsync(name, password);

            Assert.False(result, "Login should NOT be successful.");
            Assert.Equal(expectedStatus, this.client.Response.StatusCode);
        }
    }
}
