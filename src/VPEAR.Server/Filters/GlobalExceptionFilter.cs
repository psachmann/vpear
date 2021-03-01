// <copyright file="GlobalExceptionFilter.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Net;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Filters
{
    /// <summary>
    /// Catches all exceptions and returns standard messages.
    /// </summary>
    public sealed class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Filters and handles exceptions for the server.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case NotImplementedException exception:
                    NotImplementedExceptionOccurred(exception, context);
                    break;
                default:
                    ExceptionOccurred(context.Exception, context);
                    break;
            }
        }

        private static void ExceptionOccurred(Exception exception, ExceptionContext context)
        {
#if DEBUG
            throw exception;
#else
            Log.Information("{@Message}", exception.Message);
            Log.Error("{@Source}", exception.Source);
            Log.Fatal("{@Stacktrace}", exception.StackTrace);

            var response = new ErrorResponse(HttpStatusCode.InternalServerError, ErrorMessages.InternalServerError);

            context.HttpContext.Response.StatusCode = response.StatusCode;
            context.Result = new JsonResult(response);
#endif
        }

        private static void NotImplementedExceptionOccurred(NotImplementedException exception, ExceptionContext context)
        {
            var response = new ErrorResponse(HttpStatusCode.NotImplemented, exception.Message);

            context.HttpContext.Response.StatusCode = response.StatusCode;
            context.Result = new JsonResult(response);
        }
    }
}
