// <copyright file="ErrorResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class ErrorResponse
    {
        public ErrorResponse(HttpStatusCode statusCode, string message)
        {
            this.StatusCode = (int)statusCode;
            this.Messages = new List<string>() { message };
        }

        public ErrorResponse(HttpStatusCode statusCode, IEnumerable<string> messages)
        {
            this.StatusCode = (int)statusCode;
            this.Messages = new List<string>(messages);
        }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("messages")]
        public IList<string> Messages { get; set; }
    }
}
