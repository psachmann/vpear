// <copyright file="Result.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Net;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core
{
    /// <summary>
    /// Wrapper for a service result.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    public class Result<TSuccess>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TSuccess}"/> class.
        /// </summary>
        /// <param name="statusCode">The http status code.</param>
        public Result(HttpStatusCode statusCode)
        {
            this.IsSuccess = true;
            this.StatusCode = (int)statusCode;
            this.Value = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TSuccess}"/> class.
        /// </summary>
        /// <param name="statusCode">The http status code.</param>
        /// <param name="value">The success value.</param>
        public Result(HttpStatusCode statusCode, TSuccess value)
        {
            this.IsSuccess = true;
            this.StatusCode = (int)statusCode;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TSuccess}"/> class.
        /// </summary>
        /// <param name="statusCode">The http status code.</param>
        /// <param name="message">The error message.</param>
        public Result(HttpStatusCode statusCode, string message)
        {
            this.IsSuccess = false;
            this.StatusCode = (int)statusCode;
            this.Error = new ErrorResponse(statusCode, message);
        }

        /// <summary>
        /// Indicates the success of the result.
        /// </summary>
        /// <value>The success status.</value>
        public bool IsSuccess { get; }

        /// <summary>
        /// The status code for the http response.
        /// </summary>
        /// <value>The result status code.</value>
        public int StatusCode { get; }

        /// <summary>
        /// The error messages for the http response.
        /// </summary>
        /// <value>The result error.</value>
        public ErrorResponse Error { get; }

        /// <summary>
        /// The success value for the http response.
        /// </summary>
        /// <value>The success value.</value>
        public TSuccess Value { get; }
    }
}
