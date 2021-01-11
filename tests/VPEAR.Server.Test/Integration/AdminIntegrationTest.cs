// <copyright file="AdminIntegrationTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Extensions;
using VPEAR.Core.Wrappers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Integration
{
    [Collection("IntegrationTest")]
    [TestCaseOrderer("VPEAR.Server.Test.PriorityOrderer", "VPEAR.Server.Test")]
    public class AdminIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string BaseAddress = "http://localhost";
        private const string AdminName = Defaults.DefaultAdminName;
        private const string AdminPassword = Defaults.DefaultAdminPassword;
        private const string UserName = "John_Doe";
        private const string UserPassword = "password";
        private const string NewUserPassword = "new_password";

        private readonly WebApplicationFactory<Startup> factory;

        public AdminIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

/*
        public async Task Foo()
        {
            await client.DeleteDeviceAsync(string.Empty);
            await client.GetDevicesAsync();
            await client.PostDevicesAsync(string.Empty, string.Empty, string.Empty);
            await client.PutDeviceAsync(string.Empty, string.Empty, null, null);

            await client.GetFiltersAsync(string.Empty);
            await client.PutFiltersAsync(string.Empty, null, null, null);

            await client.GetFirmwareAsync(string.Empty);
            await client.PutFirmwareAsync(string.Empty, string.Empty, string.Empty, false);

            await client.GetFramesAsync(string.Empty, null, null);
            await client.GetSensorsAsync(string.Empty);

            await client.GetPowerAsync(string.Empty);

            await client.GetWifiAsync(string.Empty);
            await client.PutWifiAsync(string.Empty, string.Empty, string.Empty, null);
        }
*/
        [Priority(0)]
        [SkipIfNoDbFact]
        public async Task CanConnectAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.CanConnectAsync();

            Assert.True(result, "Should connect successful.");
        }

        [Priority(101)]
        [SkipIfNoDbFact]
        public async Task RegisterUserAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.RegisterAsync(UserName, UserPassword);

            Assert.True(result, "Register should be successful.");
            Assert.Equal(HttpStatusCode.NoContent, client.Response.StatusCode);
        }

        [Priority(102)]
        [SkipIfNoDbFact]
        public async Task VerifyUserAsync()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(AdminName, AdminPassword);

            Assert.True(result, "Login should be successful.");
            Assert.Equal(HttpStatusCode.OK, client.Response.StatusCode);

            result = await client.PutUserAsync(UserName, isVerified: true);

            Assert.True(result, "Verify should be successful.");
            Assert.Equal(HttpStatusCode.NoContent, client.Response.StatusCode);
        }

        [Priority(103)]
        [SkipIfNoDbFact]
        public async Task UpdatePasswordAsync()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(AdminName, AdminPassword);

            Assert.True(result, "Login should be successful.");
            Assert.Equal(HttpStatusCode.OK, client.Response.StatusCode);

            result = await client.PutUserAsync(UserName, UserPassword, NewUserPassword);

            Assert.True(result, "Put user should be successful.");
            Assert.Equal(HttpStatusCode.NoContent, client.Response.StatusCode);
        }

        [Priority(104)]
        [SkipIfNoDbFact]
        public async Task GetUsersAsync()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(AdminName, AdminPassword);

            Assert.True(result, "Login should be successful.");
            Assert.Equal(HttpStatusCode.OK, client.Response.StatusCode);

            var container = await client.GetUsersAsync();

            Assert.InRange(container.Count, 1, int.MaxValue);

            container = await client.GetUsersAsync(Roles.AdminRole);

            Assert.InRange(container.Count, 1, int.MaxValue);

            container = await client.GetUsersAsync(Roles.UserRole);

            Assert.InRange(container.Count, 0, int.MaxValue);
        }

        [Priority(105)]
        [SkipIfNoDbFact]
        public async Task DeleteUserAsync()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(AdminName, AdminPassword);

            Assert.True(result, "Login should be successful.");
            Assert.Equal(HttpStatusCode.OK, client.Response.StatusCode);

            result = await client.DeleteUserAsync(UserName);

            Assert.True(result, "Delete user should be successful.");
            Assert.Equal(HttpStatusCode.NoContent, client.Response.StatusCode);
        }

        private IVPEARClient CreateClient()
        {
            return new VPEARClient(BaseAddress, this.factory.CreateClient());
        }

        /*


        [Priority(10)]
        [SkipIfNoDbFact]
        public async Task DeleteUserAsync()
        {
            var result = await this.client.DeleteUserAsync(UserName);

            Assert.True(result, "Delete user should be successful.");
            Assert.Equal(HttpStatusCode.OK, this.client.Response.StatusCode);
        }
        */
    }
}
