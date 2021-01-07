// <copyright file="PostRegisterRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json request wrapper class.
    /// </summary>
    public class PostRegisterRequest
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The desired user name.</value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The desired user password.</value>
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an admin or not.
        /// </summary>
        /// <value>Indicates whether the user is an admin or not.</value>
        [JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }
    }
}
