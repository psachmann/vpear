// <copyright file="VPEARClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core
{
    public class VPEARClient : AbstractClient
    {
        private const string AuthenticationScheme = "Bearer";
        private const string BaseRoute = "/api/v1";
        private string token = string.Empty;
        private DateTimeOffset expiresAt;

        public VPEARClient(string? baseAddress)
            : base(baseAddress)
        {
        }

        public Task LoginAsync(string? email, string? password)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(string? email, string? password)
        {
            return this.RegisterAsync(null, email, password);
        }

        public Task RegisterAsync(string? name, string? email, string? password)
        {
            Func<string> uri = () => $"{BaseRoute}/user/register";

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException($"{nameof(email)} is null or empty.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"{nameof(password)} is null or empty.");
            }

            throw new NotImplementedException();
        }

        public async Task<GetFiltersResponse?> GetFiltersAsync(Guid id)
        {
            var uri = $"{BaseRoute}/filter?id={id}";

            if (await this.GetAsync(uri)
                && this.Response != null
                && this.Response.IsSuccessStatusCode)
            {
                var json = await this.Response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<GetFiltersResponse>(json);
            }
            else
            {
                return null;
            }
        }

        protected override async Task<bool> SendAsync(HttpMethod method, string uri, object? payload = null)
        {
            var message = new HttpRequestMessage()
            {
                Content = new StringContent(JsonSerializer.Serialize(payload)),
                Method = method,
                RequestUri = new Uri(uri),
            };

            message.Headers.Authorization = new AuthenticationHeaderValue(AuthenticationScheme, this.token);

            try
            {
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

        private bool IsExpired()
        {
            if (DateTimeOffset.UtcNow < this.expiresAt)
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
