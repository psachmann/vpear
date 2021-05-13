// <copyright file="VPEARClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core
{
    /// <summary>
    /// This class implements <see cref="IVPEARClient"/> interface and should
    /// provide and sdk like experience for communication with the vpear server.
    /// You can use via dependency injection with the interface or directly
    /// instantiate a vpear client.
    /// NOTE: If you use it with dependency injection register the client as singleton.
    /// <para>
    /// USAGE:
    /// Before you use anything from the client, use the login method to get an
    /// authorization token and the rest will be handled by the client.
    /// NOTE: If you are not registered already use the register method, but
    /// it will take some time, because the admin has to verify the registration.
    /// </para>
    /// </summary>
    public class VPEARClient : IVPEARClient
    {
        private const string AuthenticationScheme = "Bearer";
        private const string ApiPrefix = "/api/v1";
        private readonly string baseAddress = string.Empty;
        private readonly HttpClient client = default;
        private Exception error = default;
        private string errorMessage = string.Empty;
        private string name = string.Empty;
        private string password = string.Empty;
        private string token = string.Empty;
        private DateTimeOffset expiresAt = default;

        /// <summary>
        /// Initializes a new instance of the <see cref="VPEARClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address for the client to connect to.</param>
        /// <param name="client">The http client to create http connections.</param>
        public VPEARClient(string baseAddress, HttpClient client)
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
            var uri = $"{ApiPrefix}";

            using (var response = await this.client.GetAsync(uri))
            {
                return response.IsSuccessStatusCode;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteDeviceAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device?id={deviceId}";

            return await this.DeleteAsync(uri);
        }

        /// <inheritdoc/>
        public async Task<Container<GetDeviceResponse>> GetDevicesAsync(DeviceStatus? status = default)
        {
            var uri = $"{ApiPrefix}/device";

            if (status != null)
            {
                uri += $"?status={status}";
            }

            return await this.GetAsync<Container<GetDeviceResponse>>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PostDevicesAsync(string address, string subnetMask)
        {
            var uri = $"{ApiPrefix}/device";
            var payload = new PostDeviceRequest()
            {
                Address = address,
                SubnetMask = subnetMask,
            };

            return await this.PostAsync(uri, payload);
        }

        /// <inheritdoc/>
        public async Task<bool> PutDeviceAsync(
            string deviceId,
            string displayName = default,
            int? frequency = default,
            int? requiredSensors = default,
            DeviceStatus? status = default)
        {
            var uri = $"{ApiPrefix}/device?id={deviceId}";
            var payload = new PutDeviceRequest()
            {
                DisplayName = displayName,
                Frequency = frequency,
                RequiredSensors = requiredSensors,
                Status = status,
            };

            return await this.PutAsync(uri, payload);
        }

        /// <inheritdoc/>
        public async Task<GetFiltersResponse> GetFiltersAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/filter?id={deviceId}";

            return await this.GetAsync<GetFiltersResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PutFiltersAsync(string deviceId, bool? spot, bool? smooth, bool? noise)
        {
            var uri = $"{ApiPrefix}/device/filter?id={deviceId}";
            var payload = new PutFilterRequest()
            {
                Noise = noise,
                Smooth = smooth,
                Spot = spot,
            };

            return await this.PutAsync(uri, payload);
        }

        /// <inheritdoc/>
        public async Task<GetFirmwareResponse> GetFirmwareAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/firmware?id={deviceId}";

            return await this.GetAsync<GetFirmwareResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PutFirmwareAsync(string deviceId, string source, string upgrade, bool package = false)
        {
            var uri = $"{ApiPrefix}/device/firmware?id={deviceId}";
            var payload = new PutFirmwareRequest()
            {
                Package = package,
                Source = source,
                Upgrade = upgrade,
            };

            return await this.PutAsync(uri, payload);
        }

        /// <inheritdoc/>
        public async Task<GetPowerResponse> GetPowerAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/power?id={deviceId}";

            return await this.GetAsync<GetPowerResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<Container<GetFrameResponse>> GetFramesAsync(string deviceId, int? start, int? count)
        {
            var uri = $"{ApiPrefix}/device/frames?id={deviceId}";

            if (start != null)
            {
                uri += $"&start={start}";
            }

            if (count != null)
            {
                uri += $"&count={count}";
            }

            return await this.GetAsync<Container<GetFrameResponse>>(uri);
        }

        /// <inheritdoc/>
        public async Task<Container<GetSensorResponse>> GetSensorsAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/sensors?id={deviceId}";

            return await this.GetAsync<Container<GetSensorResponse>>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteUserAsync(string name)
        {
            var uri = $"{ApiPrefix}/user?name={name}";

            return await this.DeleteAsync(uri);
        }

        /// <inheritdoc/>
        public async Task<Container<GetUserResponse>> GetUsersAsync(string role = null)
        {
            var uri = $"{ApiPrefix}/user";

            if (role != null)
            {
                uri += $"?role={role}";
            }

            return await this.GetAsync<Container<GetUserResponse>>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PutVerifyAsync(string name, bool isVerified)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Is null or empty.", nameof(name));
            }

            var uri = $"{ApiPrefix}/user/verify?name={name}";
            var payload = new PutVerifyRequest()
            {
                IsVerified = isVerified,
            };

            return await this.PutAsync(uri, payload);
        }

        /// <inheritdoc/>
        public async Task<bool> PutPasswordAsync(string name, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Is null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException("Is null or empty.", nameof(oldPassword));
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Is null or empty.", nameof(newPassword));
            }

            var uri = $"{ApiPrefix}/user/password?name={name}";
            var payload = new PutPasswordRequest()
            {
                NewPassword = newPassword,
                OldPassword = oldPassword,
                Token = this.token,
            };

            return await this.PutAsync(uri, payload);
        }

        /// <inheritdoc/>
        public async Task<bool> LoginAsync(string name, string password)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Is null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Is null or empty.", nameof(password));
            }

            var uri = $"{ApiPrefix}/user/login";
            var payload = new PutLoginRequest()
            {
                Name = name,
                Password = password,
            };

            using (var response = await this.client.PutAsJsonAsync(uri, payload))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PutLoginResponse>();

                    this.name = name;
                    this.password = password;
                    this.token = result.Token;
                    this.expiresAt = result.ExpiresAt;
                    this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationScheme, this.token);

                    return true;
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    this.errorMessage = string.Join(" ", error.Messages);

                    return false;
                }
            }
        }

        /// <inheritdoc/>
        public void Logout()
        {
            this.name = string.Empty;
            this.password = string.Empty;
            this.token = string.Empty;
            this.expiresAt = default;
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationScheme);
        }

        /// <inheritdoc/>
        public async Task<bool> RegisterAsync(string name, string password, bool isAdmin = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Is null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Is null or empty.", nameof(password));
            }

            var uri = $"{ApiPrefix}/user/register";
            var payload = new PostRegisterRequest()
            {
                Name = name,
                Password = password,
                IsAdmin = isAdmin,
            };

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

                    return false;
                }
            }
        }

        /// <inheritdoc/>
        public async Task<GetWifiResponse> GetWifiAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/wifi?id={deviceId}";

            return await this.GetAsync<GetWifiResponse>(uri);
        }

        /// <inheritdoc/>
        public async Task<bool> PutWifiAsync(string deviceId, string ssid, string password, string mode = null)
        {
            var uri = $"{ApiPrefix}/device/wifi?id={deviceId}";
            var payload = new PutWifiRequest()
            {
                Mode = mode,
                Password = password,
                Ssid = ssid,
            };

            return await this.PutAsync(uri, payload);
        }

        private async Task<TResult> GetAsync<TResult>(string uri)
        {
            try
            {
                await this.CheckTokenAsync();

                using (var response = await this.client.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<TResult>();
                    }
                    else
                    {
                        this.errorMessage = await response.Content.ReadAsStringAsync();

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
                await this.CheckTokenAsync();

                using (var response = await this.client.DeleteAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        this.errorMessage = await response.Content.ReadAsStringAsync();

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
                await this.CheckTokenAsync();

                using (var response = await this.client.PostAsJsonAsync(uri, payload))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        this.errorMessage = await response.Content.ReadAsStringAsync();

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
                await this.CheckTokenAsync();

                using (var response = await this.client.PutAsJsonAsync(uri, payload))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        this.errorMessage = await response.Content.ReadAsStringAsync();

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

        private async Task CheckTokenAsync()
        {
            if (DateTime.UtcNow > this.expiresAt)
            {
                await this.LoginAsync(this.name, this.password);
            }
        }
    }
}
