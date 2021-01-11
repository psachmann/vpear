// <copyright file="Result.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
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
        /// <param name="errorMessage">The error message.</param>
        public Result(HttpStatusCode statusCode, string errorMessage)
        {
            this.IsSuccess = false;
            this.StatusCode = (int)statusCode;
            this.Error = new ErrorResponse(statusCode, errorMessage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TSuccess}"/> class.
        /// </summary>
        /// <param name="statusCode">The http status code.</param>
        /// <param name="errorMessages">The error messages.</param>
        public Result(HttpStatusCode statusCode, IEnumerable<string> errorMessages)
        {
            this.IsSuccess = false;
            this.StatusCode = (int)statusCode;
            this.Error = new ErrorResponse(statusCode, errorMessages);
        }

        /// <summary>
        /// Gets a value indicating whether the result is successful or not.
        /// </summary>
        /// <value>The success status.</value>
        public bool IsSuccess { get; }

        /// <summary>
        /// gets the status code for the http response.
        /// </summary>
        /// <value>The result status code.</value>
        public int StatusCode { get; }

        /// <summary>
        /// Gets the error messages for the http response.
        /// </summary>
        /// <value>The result error.</value>
        public ErrorResponse Error { get; }

        /// <summary>
        /// Gets the success value for the http response.
        /// </summary>
        /// <value>The success value.</value>
        public TSuccess Value { get; }
    }
}
