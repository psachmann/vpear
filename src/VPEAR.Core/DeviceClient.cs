// <copyright file="DeviceClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core
{
    /// <summary>
    /// Implements the <see cref="IDeviceClient"/> interface.
    /// </summary>
    public sealed class DeviceClient : IDeviceClient, IDisposable
    {
        private readonly string baseAddess;
        private readonly HttpClient client;
        private HttpStatusCode status;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceClient"/> class.
        /// </summary>
        /// <param name="baseAddess">The base url for the client to connect to.</param>
        public DeviceClient(string baseAddess)
        {
            this.baseAddess = baseAddess;
            this.client = new HttpClient();

            this.client.BaseAddress = new Uri(baseAddess);
        }

        public delegate DeviceClient Factory(string baseAddess);

        /// <inheritdoc/>
        public string BaseAddress
        {
            get { return this.baseAddess; }
        }

        /// <inheritdoc/>
        public HttpStatusCode ResponseStatusCode
        {
            get { return this.status; }
        }

        public async Task<bool> IsReachableAsync()
        {
            try
            {
                _ = await this.client.GetAsync("api");

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.client.Dispose();
        }

        /// <inheritdoc/>
        public async Task<DeviceResponse?> GetDeviceAsync()
        {
            var response = await this.client.GetAsync("api/device");
            var json = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            return JsonSerializer.Deserialize<DeviceResponse>(json);
        }

        /// <inheritdoc/>
        public async Task<GetFiltersResponse?> GetFiltersAsync()
        {
            var response = await this.client.GetAsync("api/filters");
            var json = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            return JsonSerializer.Deserialize<GetFiltersResponse>(json);
        }

        /// <inheritdoc/>
        public async Task<GetFirmwareResponse?> GetFirmwareAsync()
        {
            var response = await this.client.GetAsync("api/firmware/version");
            var version = await response.Content.ReadAsStringAsync();
            response = await this.client.GetAsync("api/firmware/upgrade");
            var upgrade = await response.Content.ReadAsStringAsync();
            response = await this.client.GetAsync("api/firmware/source");
            var source = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            return new GetFirmwareResponse()
            {
                Source = source,
                Upgrade = upgrade,
                Version = version,
            };
        }

        /// <inheritdoc/>
        public async Task<IList<FrameResponse>?> GetFramesAsync(int? after = null)
        {
            var response = after switch
            {
                null => await this.client.GetAsync("api/frames"),
                _ => await this.client.GetAsync($"api/frames?after={after}"),
            };
            var json = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            return JsonSerializer.Deserialize<List<FrameResponse>>(json);
        }

        /// <inheritdoc/>
        public async Task<int?> GetFrequencyAsync()
        {
            var response = await this.client.GetAsync("api/frequency");
            var text = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            if (int.TryParse(text, out var frequency))
            {
                return frequency;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<GetPowerResponse?> GetPowerAsync()
        {
            var response = await this.client.GetAsync("api/power");
            var json = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            return JsonSerializer.Deserialize<GetPowerResponse>(json);
        }

        /// <inheritdoc/>
        public async Task<int?> GetRequiredSensorsAsync()
        {
            var response = await this.client.GetAsync("api/sensorsRequired");
            var text = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            if (int.TryParse(text, out var requiredSensors))
            {
                return requiredSensors;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<GetSensorResponse?> GetSensorsAsync()
        {
            var response = await this.client.GetAsync("api/sensors");
            var json = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            return JsonSerializer.Deserialize<GetSensorResponse>(json);
        }

        /// <inheritdoc/>
        public Task<dynamic> GetSSEAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<DateTimeOffset?> GetTimeAsync()
        {
            var response = await this.client.GetAsync("api/time");
            var text = await response.Content.ReadAsStringAsync();
            var format = "yyyy-MM-dd HH:mm:ss.fff";
            var culture = CultureInfo.InvariantCulture;
            var style = DateTimeStyles.None;

            this.status = response.StatusCode;

            if (DateTimeOffset.TryParseExact(text, format, culture, style, out var time))
            {
                return time;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<GetWifiResponse?> GetWifiAsync()
        {
            var response = await this.client.GetAsync("api/wifi");
            var json = await response.Content.ReadAsStringAsync();

            this.status = response.StatusCode;

            return JsonSerializer.Deserialize<GetWifiResponse>(json);
        }

        /// <inheritdoc/>
        public async Task PutFiltersAsync(bool spot, bool smooth, bool noise)
        {
            var response = await this.client.PutAsync("api/filters/spot", new MultipartContent(spot.ToString().ToLower()));
            response = await this.client.PutAsync("api/filters/smooth", new MultipartContent(smooth.ToString().ToLower()));
            response = await this.client.PutAsync("api/filters/noise", new MultipartContent(noise.ToString().ToLower()));

            this.status = response.StatusCode;
        }

        /// <inheritdoc/>
        public async Task PutFirmwareAsync(string? source, string? upgrade, bool package = false)
        {
            if (source != null)
            {
                var response = await this.client.PutAsync("api/firmware/source", new MultipartContent(source));

                this.status = response.StatusCode;
            }

            if (upgrade != null)
            {
                var response = await this.client.PutAsync("api/firmware/upgrade", new MultipartContent(upgrade));

                this.status = response.StatusCode;
            }

            if (package)
            {
                var response = await this.client.PutAsync("api/firmware/package", new MultipartContent());

                this.status = response.StatusCode;
            }
        }

        /// <inheritdoc/>
        /// TODO: could frequency also be float/double?
        public async Task PutFrequencyAsync(int frequency)
        {
            var response = await this.client.PutAsync("api/frequency", new MultipartContent(frequency.ToString()));

            this.status = response.StatusCode;
        }

        /// <inheritdoc/>
        public async Task PutRequiredSensorsAsync(int requiredSensors)
        {
            var response = await this.client.PutAsync("api/sensoresRequired", new MultipartContent(requiredSensors.ToString()));

            this.status = response.StatusCode;
        }

        /// <inheritdoc/>
        public async Task PutWifiAsync(string ssid, string password, string? mode = null)
        {
            var json = $"{{\"ssid\":\"{ssid}\",\"password\":\"{password}\"}}";
            var response = await this.client.PutAsync("api/wifi", new MultipartContent(json));

            this.status = response.StatusCode;

            if (mode != null)
            {
                response = await this.client.PutAsync("api/wifi/mode", new MultipartContent(mode));

                this.status = response.StatusCode;
            }
        }
    }
}
