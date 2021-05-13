// <copyright file="PutVerifyRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json request wrapper class.
    /// </summary>
    public class PutVerifyRequest
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user is verified or not.
        /// </summary>
        /// <value>Indicates whether the user is verified or not.</value>
        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }
    }
}
