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
    /// <summary>
    /// Base class for clients. It implements some base functionality.
    /// </summary>
    public abstract class AbstractClient : IDisposable
    {
        private readonly HttpClient client;
        private Exception error;
        private HttpResponseMessage response;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address to connect to.</param>
        /// <param name="factory">The http client factory.</param>
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

            if (Uri.TryCreate(baseAddress, UriKind.Absolute, out var uri))
            {
                this.client = factory.CreateClient();
                this.client.BaseAddress = uri;
            }
            else
            {
                throw new ArgumentException("Is not a valid absolute Uri.", nameof(baseAddress));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address to connect to.</param>
        /// <param name="client">The http client.</param>
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

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The occurred client error.</value>
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

        /// <summary>
        /// Gets or sets the http response.
        /// </summary>
        /// <value>The http response for the http request.</value>
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

        /// <summary>
        /// Gets the http client.
        /// </summary>
        /// <value>The http client which executes the http requests.</value>
        protected HttpClient Client
        {
            get
            {
                return this.client;
            }
        }

        /// <summary>
        /// Indicates whether can connect to the destination or not.
        /// </summary>
        /// <returns>True if we can cannoct, otherwise false.</returns>
        public abstract Task<bool> CanConnectAsync();

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Client.Dispose();
            this.Response?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Indicates whether the response is successful or not.
        /// </summary>
        /// <returns>True if the response was successful, otherwise false.</returns>
        public bool IsSuccessResponse()
        {
            return this.Response != null && this.Response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Makes a http DELETE request to the given uri.
        /// </summary>
        /// <param name="uri">The request uri.</param>
        /// <returns>True if the request was successful, otherwise false.</returns>
        protected Task<bool> DeleteAsync(string uri)
        {
            return this.SendAsync<Null>(HttpMethod.Delete, uri);
        }

        /// <summary>
        /// Makes a http GET request to the given uri.
        /// </summary>
        /// <param name="uri">The request uri.</param>
        /// <returns>True if the request was successful, otherwise false.</returns>
        protected Task<bool> GetAsync(string uri)
        {
            return this.SendAsync<Null>(HttpMethod.Get, uri);
        }

        /// <summary>
        /// Makes a http POST request to the given uri.
        /// </summary>
        /// <param name="uri">The request uri.</param>
        /// <param name="payload">The request payload.</param>
        /// <typeparam name="TPayload">The payload type.</typeparam>
        /// <returns>True if the request was successful, otherwise false.</returns>
        protected Task<bool> PostAsync<TPayload>(string uri, TPayload payload)
        {
            return this.SendAsync(HttpMethod.Post, uri, payload);
        }

        /// <summary>
        /// Makes a http PUT request to the given uri.
        /// </summary>
        /// <param name="uri">The request uri.</param>
        /// <param name="payload">The request payload.</param>
        /// <typeparam name="TPayload">The payload type.</typeparam>
        /// <returns>True if the request was successful, otherwise false.</returns>
        protected Task<bool> PutAsync<TPayload>(string uri, TPayload payload)
        {
            return this.SendAsync(HttpMethod.Put, uri, payload);
        }

        /// <summary>
        /// Executes the http requests for the client.
        /// </summary>
        /// <param name="method">The http method.</param>
        /// <param name="uri">The request uri.</param>
        /// <param name="payload">The request payload.</param>
        /// <typeparam name="TPayload">The payload type.</typeparam>
        /// <returns>True if the request was successful, otherwise false.</returns>
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
