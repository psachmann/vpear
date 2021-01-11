// <copyright file="DeviceClientTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Extensions;
using VPEAR.Core.Wrappers;
using Xunit;

namespace VPEAR.Server.Test.Integration
{
    public class DeviceClientTest
    {
        private const string FailureBaseAddress = "http://192.168.33.1";

        [Fact]
        public void DeviceClientConstructorTest()
        {
            Assert.Throws<ArgumentNullException>(() => new DeviceClient(null, client: null));
            Assert.Throws<ArgumentNullException>(() => new DeviceClient(FailureBaseAddress, client: null));
            Assert.Throws<ArgumentNullException>(() => new DeviceClient(null, client: new HttpClient()));
            Assert.Throws<ArgumentException>(() => new DeviceClient("0.0.0.0", client: new HttpClient()));

            var factory = new Mock<IHttpClientFactory>();
            Assert.Throws<ArgumentNullException>(() => new DeviceClient(null, factory: null));
            Assert.Throws<ArgumentNullException>(() => new DeviceClient(FailureBaseAddress, factory: null));
            Assert.Throws<ArgumentNullException>(() => new DeviceClient(null, factory: factory.Object));
            Assert.Throws<ArgumentException>(() => new DeviceClient("0.0.0.0", factory: factory.Object));
        }

        [Fact]
        public async Task DeviceClientFailureTest()
        {
            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(100.0),
            };
            using var client = new DeviceClient(FailureBaseAddress, httpClient);

            Assert.False(await client.CanConnectAsync(), "There is a device?!");
            Assert.Null(await client.GetAsync());
            Assert.Null(await client.GetDeviceAsync());
            Assert.Null(await client.GetFiltersAsync());
            Assert.Null(await client.GetFirmwareAsync());
            Assert.Null(await client.GetFrequencyAsync());
            Assert.Null(await client.GetFramesAsync());
            Assert.Null(await client.GetFramesAsync(1));
            Assert.Null(await client.GetPowerAsync());
            Assert.Null(await client.GetRequiredSensorsAsync());
            Assert.Null(await client.GetSensorsAsync());
            Assert.Null(await client.GetTimeAsync());
            Assert.Null(await client.GetWifiAsync());
            Assert.False(await client.PutFiltersAsync(true, true, true));
            Assert.False(await client.PutFiltersAsync(null, true, true));
            Assert.False(await client.PutFiltersAsync(null, null, true));
            Assert.True(await client.PutFiltersAsync(null, null, null));
            Assert.False(await client.PutFirmwareAsync(string.Empty, string.Empty, true));
            Assert.False(await client.PutFirmwareAsync(null, null, true));
            Assert.False(await client.PutFirmwareAsync(null, string.Empty, true));
            Assert.False(await client.PutFirmwareAsync(null, null, true));
            Assert.False(await client.PutFirmwareAsync(null, string.Empty, true));
            Assert.False(await client.PutFirmwareAsync(null, null, true));
            Assert.True(await client.PutFirmwareAsync(null, null, false));
            Assert.False(await client.PutFrequencyAsync(100));
            Assert.True(await client.PutFrequencyAsync(null));
            Assert.False(await client.PutRequiredSensorsAsync(3));
            Assert.True(await client.PutRequiredSensorsAsync(null));
            Assert.False(await client.PutTimeAsync(DateTimeOffset.UtcNow));
            Assert.False(await client.PutWifiAsync(string.Empty, null, null));
            Assert.False(await client.PutWifiAsync(string.Empty, string.Empty, null));
            Assert.False(await client.PutWifiAsync(string.Empty, string.Empty, string.Empty));
        }

        [Fact]
        public void DeviceClientWrappersTest()
        {
            var apiResponse = new ApiResponse();
            var deviceResponse = new DeviceResponse();
            var filtersResponse = new FiltersResponse();
            var firmwareResponse = new FirmwareResponse();
            var frameResponse = new FrameResponse();
            var powerResponse = new PowerResponse();
            var sensorResponse = new SensorResponse();
            var wifiResponse = new WifiResponse();

            Assert.Equal(apiResponse.ToJsonString(), apiResponse.Clone().ToJsonString());
            Assert.Equal(deviceResponse.ToJsonString(), deviceResponse.Clone().ToJsonString());
            Assert.Equal(filtersResponse.ToJsonString(), filtersResponse.Clone().ToJsonString());
            Assert.Equal(firmwareResponse.ToJsonString(), firmwareResponse.Clone().ToJsonString());
            Assert.Equal(frameResponse.ToJsonString(), frameResponse.Clone().ToJsonString());
            Assert.Equal(powerResponse.ToJsonString(), powerResponse.Clone().ToJsonString());
            Assert.Equal(sensorResponse.ToJsonString(), sensorResponse.Clone().ToJsonString());
            Assert.Equal(wifiResponse.ToJsonString(), wifiResponse.Clone().ToJsonString());
        }
    }
}
