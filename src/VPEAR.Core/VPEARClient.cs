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
    public class VPEARClient : AbstractClient, IVPEARClient
    {
        private const string AuthenticationScheme = "Bearer";
        private const string ApiPrefix = "/api/v1";
        private bool isSignedIn = false;
        private string token = string.Empty;
        private DateTimeOffset expiresAt = default;

        public VPEARClient(string baseAddress, IHttpClientFactory factory)
            : base(baseAddress, factory)
        {
        }

        public delegate IVPEARClient Factory(string baseAddress);

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

        public async Task<bool> DeleteDeviceAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device?id={deviceId}";

            return await this.DeleteAsync(uri) && this.IsSuccessResponse();
        }

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

                return json.FromJsonString<Container<GetDeviceResponse>>();
            }
            else
            {
                return null;
            }
        }

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

        public async Task<bool> PutDeviceAsync(string deviceId, string displayName, uint? frquency, uint? requiredSesnors)
        {
            var uri = $"{ApiPrefix}/device?id={deviceId}";
            var payload = new PutDeviceRequest()
            {
                DisplayName = displayName,
                Frequency = frquency,
                RequiredSensors = requiredSesnors,
            };

            return await this.PutAsync(uri, payload) && this.IsSuccessResponse();
        }

        public async Task<GetFiltersResponse> GetFiltersAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/filter?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json.FromJsonString<GetFiltersResponse>();
            }
            else
            {
                return null;
            }
        }

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

        public async Task<GetFirmwareResponse> GetFirmwareAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/firmware?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json.FromJsonString<GetFirmwareResponse>();
            }
            else
            {
                return null;
            }
        }

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

        public async Task<GetPowerResponse> GetPowerAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/power?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json.FromJsonString<GetPowerResponse>();
            }
            else
            {
                return null;
            }
        }

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

                return json.FromJsonString<Container<GetFrameResponse>>();
            }
            else
            {
                return null;
            }
        }

        public async Task<Container<GetSensorResponse>> GetSensorsAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/sensors?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json.FromJsonString<Container<GetSensorResponse>>();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(string name)
        {
            var uri = $"{ApiPrefix}/user?name={name}";

            return await this.DeleteAsync(uri) && this.IsSuccessResponse();
        }

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

                return json.FromJsonString<Container<GetUserResponse>>();
            }
            else
            {
                return null;
            }
        }

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
                this.token = response.Token;
                this.expiresAt = response.ExpiresAt;

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Logout()
        {
            this.isSignedIn = false;
            this.token = string.Empty;
            this.expiresAt = default;
        }

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

        public async Task<GetWifiResponse> GetWifiAsync(string deviceId)
        {
            var uri = $"{ApiPrefix}/device/wifi?id={deviceId}";

            if (await this.GetAsync(uri) && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return json.FromJsonString<GetWifiResponse>();
            }
            else
            {
                return null;
            }
        }

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
                await this.RenewTokenAsync();
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

        private async Task RenewTokenAsync()
        {
            var uri = $"{ApiPrefix}/user/token";
            var payload = new PutTokenRequest()
            {
                OldToken = this.token,
            };

            if (await this.CanConnectAsync()
                && await this.PutAsync(uri, payload)
                && this.IsSuccessResponse())
            {
                var json = await this.Response.Content.ReadAsStringAsync();
                var respone = json.FromJsonString<PutTokenResponse>();

                this.token = respone.NewToken;
                this.expiresAt = respone.ExpiresAt;
            }
            else
            {
                throw new ApplicationException("The token could NOT be renewed.");
            }
        }
    }
}
