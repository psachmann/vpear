// <copyright file="PutLoginRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class PutLoginRequest
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The user name.</value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The user password.</value>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
