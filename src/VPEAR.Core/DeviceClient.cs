// <copyright file="DeviceClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Extensions;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core
{
    /// <summary>
    /// Implements the <see cref="IDeviceClient"/> interface.
    /// </summary>
    public class DeviceClient : AbstractClient, IDeviceClient
    {
        private const string ApiPrefix = "/api";
        private const string TimeFormat = "yyyy-MM-dd hh:mm:ss.fff";

        public DeviceClient(string baseAddess, IHttpClientFactory factory)
            : base(baseAddess, factory)
        {
        }

        internal DeviceClient(string baseAddress, HttpClient client)
            : base(baseAddress, client)
        {
        }

        public delegate IDeviceClient Factory(string baseAddess);

        public override async Task<bool> CanConnectAsync()
        {
            var uri = $"{ApiPrefix}/device";

            return await this.GetAsync(uri) && this.IsSuccessResponse();
        }

        public async Task<ApiResponse> GetAsync()
        {
            var uri = $"{ApiPrefix}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<ApiResponse>();
            }
            else
            {
                return null;
            }
        }

        public async Task<DeviceResponse> GetDeviceAsync()
        {
            var uri = $"{ApiPrefix}/device";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<DeviceResponse>();
            }
            else
            {
                return null;
            }
        }

        public async Task<IList<SensorResponse>> GetSensorsAsync()
        {
            var uri = $"{ApiPrefix}/sensors";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<IList<SensorResponse>>();
            }
            else
            {
                return null;
            }
        }

        public async Task<IList<FrameResponse>> GetFramesAsync(int? after = null)
        {
            var uri = $"{ApiPrefix}/frames";

            if (after != null)
            {
                uri += $"?after={after}";
            }

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<IList<FrameResponse>>();
            }
            else
            {
                return null;
            }
        }

        public async Task<int?> GetFrequencyAsync()
        {
            var uri = $"{ApiPrefix}/frequency";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<int>();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> PutFrequencyAsync(int? frequency)
        {
            var uri = $"{ApiPrefix}/frequency";

            if (frequency == null)
            {
                return true;
            }

            return await this.PutAsync(uri, frequency) && this.IsSuccessResponse();
        }

        public async Task<int?> GetRequiredSensorsAsync()
        {
            var uri = $"{ApiPrefix}/sensorsRequired";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<int>();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> PutRequiredSensorsAsync(int? requiredSensors)
        {
            var uri = $"{ApiPrefix}/sensorsRequired";

            if (requiredSensors == null)
            {
                return true;
            }

            return await this.PutAsync(uri, requiredSensors) && this.IsSuccessResponse();
        }

        public async Task<FiltersResponse> GetFiltersAsync()
        {
            var uri = $"{ApiPrefix}/filters";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<FiltersResponse>();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> PutFiltersAsync(bool? spot, bool? smooth, bool? noise)
        {
            var spotUri = $"{ApiPrefix}/filters/spot";
            var smoothUri = $"{ApiPrefix}/filters/smooth";
            var noiseUri = $"{ApiPrefix}/filters/noise";

            if (spot != null && !(await this.PutAsync(spotUri, spot) && this.IsSuccessResponse()))
            {
                return false;
            }

            if (smooth != null && !(await this.PutAsync(smoothUri, smooth) && this.IsSuccessResponse()))
            {
                return false;
            }

            if (noise != null && !(await this.PutAsync(noiseUri, noise) && this.IsSuccessResponse()))
            {
                return false;
            }

            return true;
        }

        public async Task<PowerResponse> GetPowerAsync()
        {
            var uri = $"{ApiPrefix}/power";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<PowerResponse>();
            }
            else
            {
                return null;
            }
        }

        public async Task<DateTimeOffset?> GetTimeAsync()
        {
            var uri = $"{ApiPrefix}/time";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return DateTimeOffset.ParseExact(json, TimeFormat, null);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> PutTimeAsync(DateTimeOffset time)
        {
            var uri = $"{ApiPrefix}/time";

            return await this.PutAsync(uri, time.ToString(TimeFormat));
        }

        public async Task<WifiResponse> GetWifiAsync()
        {
            var uri = $"{ApiPrefix}/wifi";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<WifiResponse>();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> PutWifiAsync(string ssid, string password = null, string mode = null)
        {
            var uri = $"{ApiPrefix}/wifi";
            var ssidUri = $"{ApiPrefix}/wifi/ssid";
            var modeUri = $"{ApiPrefix}/wifi/mode";
            var payload = new PutWifiRequest()
            {
                Ssid = ssid,
                Password = password,
            };

            if (ssid != null && password == null && !(await this.PutAsync(ssidUri, ssid) && this.IsSuccessResponse()))
            {
                return false;
            }

            if (ssid != null && password != null && !(await this.PutAsync(uri, payload) && this.IsSuccessResponse()))
            {
                return false;
            }

            if (mode != null && !(await this.PutAsync(modeUri, mode) && this.IsSuccessResponse()))
            {
                return false;
            }

            return true;
        }

        public async Task<FirmwareResponse> GetFirmwareAsync()
        {
            var uri = $"{ApiPrefix}/firmware";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<FirmwareResponse>();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> PutFirmwareAsync(string source = null, string upgrade = null, bool package = false)
        {
            var sourceUri = $"{ApiPrefix}/firmware/source";
            var upgradeUri = $"{ApiPrefix}/firmware/upgrade";
            var packageUri = $"{ApiPrefix}/firmware/package";

            if (source != null && !(await this.PutAsync(sourceUri, source) && this.IsSuccessResponse()))
            {
                return false;
            }

            if (upgrade != null && !(await this.PutAsync(upgradeUri, upgrade) && this.IsSuccessResponse()))
            {
                return false;
            }

            if (package && !(await this.PutAsync<Null>(packageUri, null) && this.IsSuccessResponse()))
            {
                return false;
            }

            return true;
        }
    }
}
