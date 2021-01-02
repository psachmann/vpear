// <copyright file="AbstractClient.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace VPEAR.Core
{
    public abstract class AbstractClient : IDisposable
    {
        private readonly HttpClient client;
        private Exception error;
        private HttpResponseMessage response;

        public AbstractClient(string baseAddress, IHttpClientFactory factory)
        {
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentException("Is null or empty.", nameof(baseAddress));
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

        public Exception Error
        {
            get { return this.error; }
            protected set { this.error = value; }
        }

        public HttpResponseMessage Response
        {
            get { return this.response; }
            protected set { this.response = value; }
        }

        protected HttpClient Client
        {
            get { return this.client; }
        }

        public void Dispose()
        {
            this.client.Dispose();
            GC.SuppressFinalize(this);
        }

        protected Task<bool> DeleteAsync(string uri)
        {
            return this.SendAsync(HttpMethod.Delete, uri);
        }

        protected Task<bool> GetAsync(string uri)
        {
            return this.SendAsync(HttpMethod.Get, uri);
        }

        protected Task<bool> PostAsync(string uri, object payload)
        {
            return this.SendAsync(HttpMethod.Post, uri, payload);
        }

        protected Task<bool> PutAsync(string uri, object payload)
        {
            return this.SendAsync(HttpMethod.Put, uri, payload);
        }

        protected async virtual Task<bool> SendAsync(HttpMethod method, string uri, object payload = null)
        {
            var message = new HttpRequestMessage()
            {
                Content = new StringContent(JsonSerializer.Serialize(payload)),
                Method = method,
                RequestUri = new Uri(uri),
            };

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
