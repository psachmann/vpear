// <copyright file="VPEARClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Extensions;
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
    /// it will take some time, because the admin has to verify the regestration.
    /// </para>
    /// </summary>
    public class VPEARClient : AbstractClient, IVPEARClient
    {
        private const string AuthenticationScheme = "Bearer";
        private const string ApiPrefix = "/api/v1";
        private bool isSignedIn = false;
        private string name = string.Empty;
        private string password = string.Empty;
        private string token = string.Empty;
        private DateTimeOffset expiresAt = default;

        /// <summary>
        /// Initializes a new instance of the <see cref="VPEARClient"/> class.
        /// NOTE: It's highly recommended to use this constructor.
        /// </summary>
        /// <param name="baseAddress">The base address for the client to connect to.</param>
        /// <param name="factory">The factory to create the <see cref="HttpClient"/> from.</param>
        public VPEARClient(string baseAddress, IHttpClientFactory factory)
            : base(baseAddress, factory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VPEARClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address for the client to connect to.</param>
        /// <param name="client">The http client to create http connections.</param>
        public VPEARClient(string baseAddress, HttpClient client)
            : base(baseAddress, client)
        {
        }

        /// <summary>
        /// Autofac create a factory method with this delegate, because
        /// the baseAddress argument isn't known during resolution time
        /// and will be later provided.
        /// </summary>
        /// <param name="baseAddress">The base address for the client to connect to.</param>
        /// <returns>An instanciated <see cref="VPEARClient"/> with all dependencies resolved.</returns>
        public delegate IVPEARClient Factory(string baseAddress);

        /// <inheritdoc/>
        public override async Task<bool> CanConnectAsync()
        {
            var uri = $"{ApiPrefix}";

            if (await this.GetAsync(uri)
                && this.IsSuccessResponse())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteDeviceAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device?id={deviceId}";

            return await this.DeleteAsync(uri) && this.IsSuccessResponse();
        }

        /// <inheritdoc/>
        public async Task<Container<GetDeviceResponse>> GetDevicesAsync(DeviceStatus? status = null)
        {
            var uri = $"{ApiPrefix}/device";

            if (status != null)
            {
                uri += $"?status={status}";
            }

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<Container<GetDeviceResponse>>();
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> PostDevicesAsync(string deviceId, string address, string subnetMask)
        {
            var uri = $"{ApiPrefix}/device?id={deviceId}";
            var payload = new PostDeviceRequest()
            {
                Address = address,
                SubnetMask = subnetMask,
            };

            return await this.PostAsync(uri, payload) && this.IsSuccessResponse();
        }

        /// <inheritdoc/>
        public async Task<bool> PutDeviceAsync(string deviceId, string displayName, int? frequency, int? requiredSensors)
        {
            var uri = $"{ApiPrefix}/device?id={deviceId}";
            var payload = new PutDeviceRequest()
            {
                DisplayName = displayName,
                Frequency = frequency,
                RequiredSensors = requiredSensors,
            };

            return await this.PutAsync(uri, payload) && this.IsSuccessResponse();
        }

        /// <inheritdoc/>
        public async Task<GetFiltersResponse> GetFiltersAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/filter?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<GetFiltersResponse>();
            }
            else
            {
                return null;
            }
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

            return await this.PostAsync(uri, payload) && this.IsSuccessResponse();
        }

        /// <inheritdoc/>
        public async Task<GetFirmwareResponse> GetFirmwareAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/firmware?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<GetFirmwareResponse>();
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> PutFirmwareAsync(string deviceId, string source, string upgrade, bool package = false)
        {
            var uri = $"{ApiPrefix}/firmware?id={deviceId}";
            var payload = new PutFirmwareRequest()
            {
                Package = package,
                Source = source,
                Upgrade = upgrade,
            };

            return await this.PutAsync(uri, payload) && this.IsSuccessResponse();
        }

        /// <inheritdoc/>
        public async Task<GetPowerResponse> GetPowerAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/power?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<GetPowerResponse>();
            }
            else
            {
                return null;
            }
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

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<Container<GetFrameResponse>>();
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<Container<GetSensorResponse>> GetSensorsAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/sensors?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<Container<GetSensorResponse>>();
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteUserAsync(string name)
        {
            var uri = $"{ApiPrefix}/user?name={name}";

            return await this.DeleteAsync(uri) && this.IsSuccessResponse();
        }

        /// <inheritdoc/>
        public async Task<Container<GetUserResponse>> GetUsersAsync(string role = null)
        {
            var uri = $"{ApiPrefix}/user";

            if (role != null)
            {
                uri += $"?role={role}";
            }

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<Container<GetUserResponse>>();
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> LoginAsync(string name, string password)
        {
            var uri = $"{ApiPrefix}/user/login";
            var payload = new PutLoginRequest()
            {
                Name = name,
                Password = password,
            };

            if (await this.PutAsync(uri, payload) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();
                var response = json.FromJsonString<PutLoginResponse>();

                this.isSignedIn = true;
                this.name = name;
                this.password = password;
                this.token = response.Token;
                this.expiresAt = response.ExpiresAt;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Logout()
        {
            this.isSignedIn = false;
            this.name = string.Empty;
            this.password = string.Empty;
            this.token = string.Empty;
            this.expiresAt = default;
        }

        /// <inheritdoc/>
        public async Task<bool> RegisterAsync(string name, string password, bool isAdmin = false)
        {
            var uri = $"{ApiPrefix}/user/register";
            var payload = new PostRegisterRequest()
            {
                Name = name,
                Password = password,
                IsAdmin = isAdmin,
            };

            return await this.PostAsync(uri, payload) && this.IsSuccessResponse();
        }

        /// <inheritdoc/>
        public async Task<GetWifiResponse> GetWifiAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/wifi?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json?.FromJsonString<GetWifiResponse>();
            }
            else
            {
                return null;
            }
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

            return await this.PutAsync(uri, payload) && this.IsSuccessResponse();
        }

        /// <inheritdoc/>
        protected override async Task<bool> SendAsync<TPayload>(HttpMethod method, string uri, TPayload payload = default)
        {
            try
            {
                var message = new HttpRequestMessage()
                {
                    Content = new StringContent(payload?.ToJsonString()),
                    Method = method,
                    RequestUri = new Uri(uri),
                };

                if (this.isSignedIn)
                {
                    await this.CheckAndRenewTokenAsync();

                    message.Headers.Authorization = new AuthenticationHeaderValue(AuthenticationScheme, this.token);
                }

                this.Response = await this.Client.SendAsync(message);
                this.Error = null;

                return true;
            }
            catch (Exception exception)
            {
                this.Error = exception;
                this.Response = null;

                return false;
            }
        }

        private async Task CheckAndRenewTokenAsync()
        {
            if (!this.isSignedIn)
            {
                throw new ApplicationException("User is NOT signed in.");
            }

            if (this.IsTokenExpired())
            {
                await this.LoginAsync(this.name, this.password);
            }
        }

        private bool IsTokenExpired()
        {
            if (DateTimeOffset.Now < this.expiresAt)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
