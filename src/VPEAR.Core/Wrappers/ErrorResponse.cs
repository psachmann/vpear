// <copyright file="ErrorResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class ErrorResponse
    {
        public ErrorResponse(string code, string reason)
        {
            this.Code = code;
            this.Reason = reason;
        }

        public string Code { get; }

        public string Reason { get; }
    }
}
