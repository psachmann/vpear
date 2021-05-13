// <copyright file="DeviceClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core
{
    /// <summary>
    /// Implements the <see cref="IDeviceClient"/> interface.
    /// This client gives access to a sensor device.
    /// NOTE: This class is only for the vpear server to connect to devices.
    /// </summary>
    public class DeviceClient : IDeviceClient, IDisposable
    {
        private const string ApiPrefix = "/api";
        private const string TimeFormat = "yyyy-MM-dd hh:mm:ss.fff";
        private readonly string baseAddress;
        private Exception error;
        private string errorMessage;
        private HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address to connect to.</param>
        /// <param name="factory">The http client factory.</param>
        public DeviceClient(string baseAddress, IHttpClientFactory factory)
        {
            this.baseAddress = baseAddress ?? throw new ArgumentNullException(nameof(baseAddress));
            this.client = factory?.CreateClient("default") ?? throw new ArgumentNullException(nameof(factory));

            if (Uri.TryCreate(baseAddress, UriKind.Absolute, out var uri))
            {
                this.client.BaseAddress = uri;
            }
            else
            {
                throw new ArgumentException("Is not a valid Uri.", nameof(baseAddress));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address to connect to.</param>
        /// <param name="client">The http client.</param>
        internal DeviceClient(string baseAddress, HttpClient client)
        {
            this.baseAddress = baseAddress ?? throw new ArgumentNullException(nameof(baseAddress));
            this.client = client ?? throw new ArgumentNullException(nameof(client));

            if (Uri.TryCreate(baseAddress, UriKind.Absolute, out var uri))
            {
                this.client.BaseAddress = uri;
            }
            else
            {
                throw new ArgumentException("Is not a valid Uri.", nameof(baseAddress));
            }
        }

        /// <summary>
        /// Autofac create a factory method with this delegate, because
        /// the baseAddress argument isn't known during resolution time
        /// and will be later provided.
        /// </summary>
        /// <param name="baseAddress">The base address for the client to connect to.</param>
        /// <returns>An instanciated <see cref="DeviceClient"/> with all dependencies resolved.</returns>
        public delegate IDeviceClient Factory(string baseAddress);

        /// <inheritdoc/>
        public Exception Error
        {
            get { return this.error; }
        }

        /// <inheritdoc/>
        public string ErrorMessage
        {
            get { return this.errorMessage; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.client.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async Task<bool> CanConnectAsync()
        {
            var uri = $"{ApiPrefix}/device";

            return await this.GetAsync<DeviceResponse>(uri) != null;
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> GetAsync()
        {
            var uri = $"{ApiPrefix}";

            return await this.GetAsync<ApiResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<DeviceResponse> GetDeviceAsync()
        {
            var uri = $"{ApiPrefix}/device";

            return await this.GetAsync<DeviceResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<IList<SensorResponse>> GetSensorsAsync()
        {
            var uri = $"{ApiPrefix}/sensors";

            return await this.GetAsync<IList<SensorResponse>>(uri);
        }

        /// <inheritdoc/>
        public async Task<IList<FrameResponse>> GetFramesAsync(int? after = null)
        {
            var uri = $"{ApiPrefix}/frames";

            if (after != null)
            {
                uri += $"?after={after.Value}";
            }

            return await this.GetAsync<IList<FrameResponse>>(uri);
        }

        /// <inheritdoc/>
        public async Task<int?> GetFrequencyAsync()
        {
            var uri = $"{ApiPrefix}/frequency";

            return await this.GetAsync<int?>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PutFrequencyAsync(int? frequency)
        {
            var uri = $"{ApiPrefix}/frequency";

            if (frequency == null)
            {
                return true;
            }

            return await this.PutAsync(uri, frequency);
        }

        /// <inheritdoc/>
        public async Task<int?> GetRequiredSensorsAsync()
        {
            var uri = $"{ApiPrefix}/sensorsRequired";

            return await this.GetAsync<int?>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PutRequiredSensorsAsync(int? requiredSensors)
        {
            var uri = $"{ApiPrefix}/sensorsRequired";

            if (requiredSensors == null)
            {
                return true;
            }

            return await this.PutAsync(uri, requiredSensors);
        }

        /// <inheritdoc/>
        public async Task<FiltersResponse> GetFiltersAsync()
        {
            var uri = $"{ApiPrefix}/filters";

            return await this.GetAsync<FiltersResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PutFiltersAsync(bool? spot, bool? smooth, bool? noise)
        {
            var spotUri = $"{ApiPrefix}/filters/spot";
            var smoothUri = $"{ApiPrefix}/filters/smooth";
            var noiseUri = $"{ApiPrefix}/filters/noise";

            if (spot != null && !(await this.PutAsync(spotUri, spot)))
            {
                return false;
            }

            if (smooth != null && !(await this.PutAsync(smoothUri, smooth)))
            {
                return false;
            }

            if (noise != null && !(await this.PutAsync(noiseUri, noise)))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<PowerResponse> GetPowerAsync()
        {
            var uri = $"{ApiPrefix}/power";

            return await this.GetAsync<PowerResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<DateTimeOffset?> GetTimeAsync()
        {
            var uri = $"{ApiPrefix}/time";
            var result = await this.GetAsync<string>(uri);

            if (result != null)
            {
                return DateTime.ParseExact(this.CleanJsonTimeString(result), TimeFormat, CultureInfo.InvariantCulture.DateTimeFormat);
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> PutTimeAsync(DateTimeOffset time)
        {
            var uri = $"{ApiPrefix}/time";

            return await this.PutAsync(uri, time.ToString(TimeFormat));
        }

        /// <inheritdoc/>
        public async Task<WifiResponse> GetWifiAsync()
        {
            var uri = $"{ApiPrefix}/wifi";

            return await this.GetAsync<WifiResponse>(uri);
        }

        /// <inheritdoc/>
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

            if (ssid != null && password == null && !(await this.PutAsync(ssidUri, ssid)))
            {
                return false;
            }

            if (ssid != null && password != null && !(await this.PutAsync(uri, payload)))
            {
                return false;
            }

            if (mode != null && !(await this.PutAsync(modeUri, mode)))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<FirmwareResponse> GetFirmwareAsync()
        {
            var uri = $"{ApiPrefix}/firmware";

            return await this.GetAsync<FirmwareResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PutFirmwareAsync(string source = null, string upgrade = null, bool package = false)
        {
            var sourceUri = $"{ApiPrefix}/firmware/source";
            var upgradeUri = $"{ApiPrefix}/firmware/upgrade";
            var packageUri = $"{ApiPrefix}/firmware/package";

            if (source != null && !(await this.PutAsync(sourceUri, source)))
            {
                return false;
            }

            if (upgrade != null && !(await this.PutAsync(upgradeUri, upgrade)))
            {
                return false;
            }

            if (package && !(await this.PutAsync<Null>(packageUri, null)))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> SyncAsync(Device device, IRepository<Device, Guid> devices)
        {
            if (!await this.PutFiltersAsync(device.Filter.Spot, device.Filter.Smooth, device.Filter.Noise))
            {
                return false;
            }

            if (!await this.PutFrequencyAsync(device.Frequency))
            {
                return false;
            }

            if (!await this.PutRequiredSensorsAsync(device.RequiredSensors))
            {
                return false;
            }

            if (!await this.PutTimeAsync(DateTimeOffset.UtcNow))
            {
                return false;
            }

            return true;
        }

        private async Task<TResult> GetAsync<TResult>(string uri)
        {
            try
            {
                using (var response = await this.client.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<TResult>();
                    }
                    else
                    {
                        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        this.errorMessage = string.Join(" ", error.Messages);

                        return default;
                    }
                }
            }
            catch (Exception exception)
            {
                this.error = exception;
                this.errorMessage = exception.Message;

                return default;
            }
        }

        private async Task<bool> DeleteAsync(string uri)
        {
            try
            {
                using (var response = await this.client.DeleteAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        this.errorMessage = string.Join(" ", error.Messages);

                        return default;
                    }
                }
            }
            catch (Exception exception)
            {
                this.error = exception;
                this.errorMessage = exception.Message;

                return default;
            }
        }

        private async Task<bool> PostAsync<TPayload>(string uri, TPayload payload)
        {
            try
            {
                using (var response = await this.client.PostAsJsonAsync(uri, payload))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        this.errorMessage = string.Join(" ", error.Messages);

                        return default;
                    }
                }
            }
            catch (Exception exception)
            {
                this.error = exception;
                this.errorMessage = exception.Message;

                return default;
            }
        }

        private async Task<bool> PutAsync<TPayload>(string uri, TPayload payload)
        {
            try
            {
                using (var response = await this.client.PutAsJsonAsync(uri, payload))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        this.errorMessage = string.Join(" ", error.Messages);

                        return default;
                    }
                }
            }
            catch (Exception exception)
            {
                this.error = exception;
                this.errorMessage = exception.Message;

                return default;
            }
        }

        private string CleanJsonTimeString(string json)
        {
            var time = json.Replace("\"", string.Empty);

            if (time.Length > TimeFormat.Length)
            {
                time = time.Remove(time.Length - 1);
            }

            return time;
        }
    }
}
