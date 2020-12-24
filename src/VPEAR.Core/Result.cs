// <copyright file="Result.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Net;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core
{
    public class Result<TSuccess>
    {
        public Result(HttpStatusCode statusCode, TSuccess? value = default)
        {
            this.IsSuccess = true;
            this.StatusCode = (int)statusCode;
            this.Value = value;
        }

        public Result(HttpStatusCode statusCode, string message)
        {
            this.IsSuccess = false;
            this.StatusCode = (int)statusCode;
            this.Error = new ErrorResponse(statusCode, message);
        }

        public bool IsSuccess { get; }

        public int StatusCode { get; }

        public ErrorResponse? Error { get; }

        public TSuccess? Value { get; }
    }
}
