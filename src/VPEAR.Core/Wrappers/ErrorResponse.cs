// <copyright file="ErrorResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The http status code.</param>
        /// <param name="message">The error message.</param>
        public ErrorResponse(HttpStatusCode statusCode, string message)
        {
            this.StatusCode = (int)statusCode;
            this.Messages = new List<string>() { message };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The http status code.</param>
        /// <param name="messages">The error messages.</param>
        public ErrorResponse(HttpStatusCode statusCode, IEnumerable<string> messages)
        {
            this.StatusCode = (int)statusCode;
            this.Messages = new List<string>(messages);
        }

        /// <summary>
        /// Gets or sets the error status code for the http status code.
        /// </summary>
        /// <value>The error status code.</value>
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the error messages for the http response.
        /// </summary>
        /// <value>The error messages.</value>
        [JsonPropertyName("messages")]
        public IList<string> Messages { get; set; }
    }
}
