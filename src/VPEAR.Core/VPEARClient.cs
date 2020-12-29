// <copyright file="VPEARClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core
{
    public class VPEARClient : IDisposable
    {
        private const string AuthenticationScheme = "Bearer";
        private const string BaseRoute = "/api/v1";
        private readonly HttpClient client = new HttpClient();
        private string token = string.Empty;
        private DateTimeOffset expiresAt;
        private Exception? error;
        private HttpResponseMessage? response;

        public VPEARClient(string baseAddress)
        {
            this.client.BaseAddress = new Uri(baseAddress);
        }

        public Exception? Error
        {
            get { return this.error; }
        }

        public HttpResponseMessage? Response
        {
            get { return this.response; }
        }

        public void Dispose()
        {
            this.client.Dispose();
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
            Func<Guid, string> uri = id => $"{BaseRoute}/filter?id={id}";

            if (await this.GetAsync(uri(id))
                && this.response != null
                && this.response.StatusCode == HttpStatusCode.OK)
            {
                var json = await this.response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<GetFiltersResponse>(json);
            }
            else
            {
                return null;
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

        private Task<bool> DeleteAsync(string uri)
        {
            return this.SendAsync(HttpMethod.Delete, uri);
        }

        private Task<bool> GetAsync(string uri)
        {
            return this.SendAsync(HttpMethod.Get, uri);
        }

        private Task<bool> PostAsync(string uri, object payload)
        {
            return this.SendAsync(HttpMethod.Post, uri, payload);
        }

        private Task<bool> PutAsync(string uri, object payload)
        {
            return this.SendAsync(HttpMethod.Put, uri, payload);
        }

        private async Task<bool> SendAsync(HttpMethod method, string uri, object? payload = null)
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
                this.response = await this.client.SendAsync(message);
                this.error = null;

                return true;
            }
            catch (Exception exception)
            {
                this.error = exception;
                this.response = null;

                return false;
            }
        }
    }
}
