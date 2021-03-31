// <copyright file="VPEARClientTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Server.Data;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    [Collection("IntegrationTest")]
    [TestCaseOrderer("VPEAR.Server.Test.PriorityOrderer", "VPEAR.Server.Test")]
    public class VPEARClientTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string DeviceBaseAddress = "http://10.0.0.1";
        private const string ServerBaseAddress = "http://localhost";
        private const string AdminName = Defaults.DefaultAdminName;
        private const string AdminPassword = Defaults.DefaultAdminPassword;
        private const string UserName = "John_Doe";
        private const string UserPassword = "password";
        private const string NewUserPassword = "new_password";
        private const string NotFoundId = "00000000-0000-0000-1111-000000000000";

        private readonly WebApplicationFactory<Startup> factory;
        private readonly IList<Device> archivedDevices;
        private readonly IList<Device> notReachableDevices;
        private readonly IList<Device> stoppedDevices;

        public VPEARClientTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.archivedDevices = DataSeed.Devices.Select(device => device)
                .Where(device => device.Status == DeviceStatus.Archived)
                .ToList();
            this.notReachableDevices = DataSeed.Devices.Select(device => device)
                .Where(device => device.Status == DeviceStatus.NotReachable)
                .ToList();
            this.stoppedDevices = DataSeed.Devices.Select(device => device)
                .Where(device => device.Status == DeviceStatus.Stopped)
                .ToList();
        }

        [Priority(0)]
        [SkipIfNoDbFact]
        public async Task CanConnectAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.CanConnectAsync();

            Assert.True(result, "Should connect successful.");
        }

        [Priority(100)]
        [SkipIfNoDbTheory]
        [InlineData("no_valid_user", "password")]
        [InlineData("admin", "no_valid_password")]
        public async Task LoginAsyncFailureTest(string? name, string? password)
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(name, password);

            Assert.False(result, "Login should NOT be successful.");

            client.Logout();
        }

        [Priority(100)]
        [SkipIfNoDbTheory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("", "")]
        public void LoginAsyncThrowsTest(string? name, string? password)
        {
            using var client = this.CreateClient();
            Assert.ThrowsAsync<ArgumentException>(async () => await client.LoginAsync(name, password));
        }

        [Priority(100)]
        [SkipIfNoDbFact]
        public async Task RegisterAsyncFailureTest()
        {
            using var client = this.CreateClient();
            var result = await client.RegisterAsync(AdminName, AdminPassword);

            Assert.False(result, "Register should NOT be successful.");
        }

        [Priority(100)]
        [SkipIfNoDbTheory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("", "")]
        public void RegisterAsyncThrowsTest(string? name, string? password)
        {
            using var client = this.CreateClient();
            Assert.ThrowsAsync<ArgumentException>(async () => await client.RegisterAsync(name, password));
        }

        [Priority(100)]
        [SkipIfNoDbFact]
        public async Task LoginAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(AdminName, AdminPassword);

            Assert.True(result, "Login should be successful.");
        }

        [Priority(101)]
        [SkipIfNoDbFact]
        public async Task RegisterAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.RegisterAsync(UserName, UserPassword);

            Assert.True(result, "Register should be successful.");
        }

        [Priority(102)]
        [SkipIfNoDbFact]
        public async Task NotVerifiedAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(UserName, UserPassword);

            Assert.False(result, "Login should NOT be successful.");
        }

        [Priority(103)]
        [SkipIfNoDbFact]
        public async Task VerifyAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(AdminName, AdminPassword);

            Assert.True(result, client.ErrorMessage);

            result = await client.PutVerifyAsync(UserName, true);

            Assert.True(result, client.ErrorMessage);
        }

        [Priority(104)]
        [SkipIfNoDbFact]
        public async Task UpdatePasswordAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(UserName, UserPassword);

            Assert.True(result, "Login should be successful.");

            result = await client.PutPasswordAsync(UserName, UserPassword, NewUserPassword);

            Assert.True(result, "Put user should be successful.");
        }

        [Priority(105)]
        [SkipIfNoDbFact]
        public async Task GetUsersAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(AdminName, AdminPassword);

            Assert.True(result, "Login should be successful.");

            var container = await client.GetUsersAsync();

            Assert.InRange(container.Count, 0, int.MaxValue);

            container = await client.GetUsersAsync(Roles.AdminRole);

            Assert.InRange(container.Count, 0, int.MaxValue);

            container = await client.GetUsersAsync(Roles.UserRole);

            Assert.InRange(container.Count, 0, int.MaxValue);
        }

        [Priority(1000)]
        [SkipIfNoDbFact]
        public async Task DeleteUserAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(AdminName, AdminPassword);

            Assert.True(result, "Login should be successful.");

            result = await client.DeleteUserAsync(UserName);

            Assert.True(result, "Delete user should be successful.");
        }

        [Priority(200)]
        [SkipIfNoDbFact]
        public async Task DeleteDeviceAsyncFailure()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.DeleteDeviceAsync(NotFoundId);

            Assert.False(result);

            result = await client.DeleteDeviceAsync(this.archivedDevices.First().Id.ToString());

            Assert.False(result);
        }

        [Priority(200)]
        [SkipIfNoDbFact]
        public async Task PutDeviceAsyncFailure()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.PutDeviceAsync(NotFoundId);

            Assert.False(result);

            result = await client.PutDeviceAsync(this.archivedDevices.First().Id.ToString());

            Assert.False(result);
        }

        [Priority(201)]
        [SkipIfNoDbFact]
        public async Task GetDevicesAsyncTest()
        {
            using var client = this.CreateClient();
            var result = await client.LoginAsync(UserName, NewUserPassword);

            Assert.True(result, "Login should be successful.");

            var devices = await client.GetDevicesAsync();

            Assert.NotNull(devices);
        }

        [Priority(210)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task PutDeviceAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.PutDeviceAsync(await GetDeviceIdAsync(client), "Test Device", 3600, 1);

            Assert.True(result);
        }

        [Priority(203)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task PostDeviceAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.PostDevicesAsync("10.0.0.1", "255.255.255.0");

            Assert.True(result);
        }

        [Priority(1000)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task DeleteDeviceAsync()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.DeleteDeviceAsync(await GetDeviceIdAsync(client));

            Assert.True(result);
        }

        [Priority(300)]
        [SkipIfNoDbFact]
        public async Task GetFiltersAsyncFailuresTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.GetFiltersAsync(NotFoundId);

            Assert.Null(result);
        }

        [Priority(300)]
        public async Task PutFiltersAsyncFailureTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.PutFiltersAsync(NotFoundId, true, true, true);

            Assert.False(result);

            result = await client.PutFiltersAsync(this.archivedDevices.First().Id.ToString(), true, true, true);

            Assert.False(result);

            result = await client.PutFiltersAsync(this.notReachableDevices.First().Id.ToString(), true, true, true);

            Assert.False(result);
        }

        [Priority(301)]
        [SkipIfNoDbFact]
        public async Task GetFiltersAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(AdminName, AdminPassword);

            var result = await client.GetFiltersAsync(this.stoppedDevices.First().Id.ToString());

            Assert.NotNull(result);
        }

        [Priority(302)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task PutFiltersAsync()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.PutFiltersAsync(await GetDeviceIdAsync(client), true, true, true);

            Assert.True(result);
        }

        [Priority(400)]
        [SkipIfNoDbFact]
        public async Task GetFirmwareAsyncFailureTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.GetFirmwareAsync(NotFoundId);

            Assert.Null(result);

            result = await client.GetFirmwareAsync(this.archivedDevices.First().Id.ToString());

            Assert.Null(result);

            result = await client.GetFirmwareAsync(this.notReachableDevices.First().Id.ToString());

            Assert.Null(result);
        }

        [Priority(400)]
        [SkipIfNoDbFact]
        public async Task PutFirmwareAsyncFailureTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(AdminName, AdminPassword);

            var result = await client.PutFirmwareAsync(NotFoundId, null, null);

            Assert.False(result);

            result = await client.PutFirmwareAsync(this.archivedDevices.First().Id.ToString(), null, null);

            Assert.False(result);

            result = await client.PutFirmwareAsync(this.notReachableDevices.First().Id.ToString(), null, null);

            Assert.False(result);
        }

        [Priority(701)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task GetFirmwareAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = client.GetFirmwareAsync(await GetDeviceIdAsync(client));

            Assert.NotNull(result);
        }

/*
        [Priority(702)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task PutFirmwareAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.PutFirmwareAsync(this.stoppedDevices.First().Id.ToString(), "stable", "next", true);

            Assert.True(result, "Put wifi should be successful.");
            Assert.Equal(HttpStatusCode.NoContent, client.Response.StatusCode);
        }
*/

        [Priority(500)]
        [SkipIfNoDbFact]
        public async Task GetFramesAsyncFailureTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.GetFramesAsync(NotFoundId, null, null);

            Assert.Null(result);
        }

        [Priority(500)]
        [SkipIfNoDbFact]
        public async Task GetSensorsAsyncFailureTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.GetSensorsAsync(NotFoundId);

            Assert.Null(result);

            result = await client.GetSensorsAsync(this.archivedDevices.First().Id.ToString());

            Assert.Null(result);

            result = await client.GetSensorsAsync(this.notReachableDevices.First().Id.ToString());

            Assert.Null(result);
        }

        [Priority(501)]
        [SkipIfNoDbFact]
        public async Task GetFramesAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.GetFramesAsync(this.archivedDevices[0].Id.ToString(), 0, 1);

            Assert.NotNull(result);
            Assert.InRange(result.Items.Count, 1, int.MaxValue);
            Assert.InRange(result.Items[0].Readings.Count, 1, int.MaxValue);
        }

        [Priority(502)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task GetSensorsAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.GetSensorsAsync(await GetDeviceIdAsync(client));

            Assert.NotNull(result);
        }

        [Priority(600)]
        [SkipIfNoDbFact]
        public async Task GetPowerAsyncFailureTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.GetPowerAsync(NotFoundId);

            Assert.Null(result);

            result = await client.GetPowerAsync(this.archivedDevices.First().Id.ToString());

            Assert.Null(result);
            result = await client.GetPowerAsync(this.notReachableDevices.First().Id.ToString());

            Assert.Null(result);
        }

        [Priority(601)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task GetPowerAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = client.GetPowerAsync(await GetDeviceIdAsync(client));

            Assert.NotNull(result);
        }

        [Priority(700)]
        [SkipIfNoDbFact]
        public async Task GetWifiAsyncFailureTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.GetWifiAsync(NotFoundId);

            Assert.Null(result);

            result = await client.GetWifiAsync(this.archivedDevices.First().Id.ToString());

            Assert.Null(result);

            result = await client.GetWifiAsync(this.notReachableDevices.First().Id.ToString());

            Assert.Null(result);
        }

        [Priority(700)]
        [SkipIfNoDbFact]
        public async Task PutWifiAsyncFailureTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.PutWifiAsync(NotFoundId, "ssid", "password");

            Assert.False(result);

            result = await client.PutWifiAsync(this.archivedDevices.First().Id.ToString(), "ssid", "password");

            Assert.False(result);

            result = await client.PutWifiAsync(this.notReachableDevices.First().Id.ToString(), "ssid", "password");

            Assert.False(result);
        }

        [Priority(701)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task GetWifiAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = client.GetWifiAsync(await GetDeviceIdAsync(client));

            Assert.NotNull(result);
        }

        [Priority(702)]
        [SkipIfNoDbOrDeviceFact(DeviceBaseAddress)]
        public async Task PutWifiAsyncTest()
        {
            using var client = this.CreateClient();
            await client.LoginAsync(UserName, NewUserPassword);

            var result = await client.PutWifiAsync(await GetDeviceIdAsync(client), null, null, "direct");

            Assert.True(result, "Put wifi should be successful.");
        }

        private static async Task<string> GetDeviceIdAsync(IVPEARClient client)
        {
            var devices = await client.GetDevicesAsync(DeviceStatus.Stopped);
            var device = devices.Items.First(device => device.Address == DeviceBaseAddress);

            return device.Id;
        }

        private IVPEARClient CreateClient()
        {
            return new VPEARClient(ServerBaseAddress, this.factory.CreateClient());
        }
    }
}
