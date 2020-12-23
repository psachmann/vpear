// <copyright file="Result.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Net;

namespace VPEAR.Core.Models
{
    public class Result<TSuccess, TError>
    {
        public Result(HttpStatusCode statusCode, TSuccess value = default)
        {
            this.IsSuccess = true;
            this.StatusCode = (int)statusCode;
            this.Value = value;
        }

        public Result(HttpStatusCode statusCode, TError error = default)
        {
            this.IsSuccess = false;
            this.StatusCode = (int)statusCode;
            this.Error = error;
        }

        public bool IsSuccess { get; }

        public int StatusCode { get; }

        public TError? Error { get; }

        public TSuccess? Value { get; }
    }
}
