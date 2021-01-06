// <copyright file="AbstractClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace VPEAR.Core.Abstractions
{
    public abstract class AbstractClient : IDisposable
    {
        private readonly HttpClient client;
        private Exception error;
        private HttpResponseMessage response;

        protected AbstractClient(string baseAddress, IHttpClientFactory factory)
        {
            if (baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (Uri.TryCreate(baseAddress, UriKind.RelativeOrAbsolute, out var uri))
            {
                this.client = factory.CreateClient();
                this.client.BaseAddress = uri;
            }
            else
            {
                throw new ArgumentException("Is not a valid Uri.", nameof(baseAddress));
            }
        }

        protected AbstractClient(string baseAddress, HttpClient client)
        {
            if (baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (Uri.TryCreate(baseAddress, UriKind.RelativeOrAbsolute, out var uri))
            {
                this.client = client;
                this.client.BaseAddress = uri;
            }
            else
            {
                throw new ArgumentException("Is not a valid Uri.", nameof(baseAddress));
            }
        }

        public Exception Error
        {
            get
            {
                return this.error;
            }

            protected set
            {
                this.error = value;
            }
        }

        public HttpResponseMessage Response
        {
            get
            {
                return this.response;
            }

            protected set
            {
                this.response?.Dispose();
                this.response = value;
            }
        }

        protected HttpClient Client
        {
            get
            {
                return this.client;
            }
        }

        public abstract Task<bool> CanConnectAsync();

        public void Dispose()
        {
            this.Client.Dispose();
            this.Response?.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool IsSuccessResponse()
        {
            return this.Response != null && this.Response.IsSuccessStatusCode;
        }

        protected Task<bool> DeleteAsync(string uri)
        {
            return this.SendAsync<Null>(HttpMethod.Delete, uri);
        }

        protected Task<bool> GetAsync(string uri)
        {
            return this.SendAsync<Null>(HttpMethod.Get, uri);
        }

        protected Task<bool> PostAsync<TPayload>(string uri, TPayload payload)
        {
            return this.SendAsync(HttpMethod.Post, uri, payload);
        }

        protected Task<bool> PutAsync<TPayload>(string uri, TPayload payload)
        {
            return this.SendAsync(HttpMethod.Put, uri, payload);
        }

        protected virtual async Task<bool> SendAsync<TPayload>(HttpMethod method, string uri, TPayload payload = default)
        {
            try
            {
                var message = new HttpRequestMessage()
                {
                    Content = new StringContent(JsonSerializer.Serialize(payload)),
                    Method = method,
                    RequestUri = new Uri(uri),
                };

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
    }
}
