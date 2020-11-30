// <copyright file="Response.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Net;

namespace VPEAR.Core
{
    public class Response
    {
        public Response(HttpStatusCode statusCode, dynamic payload)
        {
            this.StatusCode = (int)statusCode;
            this.Payload = payload;
        }

        public int StatusCode { get; }

        public dynamic Payload { get; }
    }
}
