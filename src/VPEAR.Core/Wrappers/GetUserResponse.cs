// <copyright file="GetUserResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class GetUserResponse
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The user id.</value>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The user name.</value>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <returns>The user roles.</returns>
        [JsonPropertyName("roles")]
        public IList<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the user is verified or not.
        /// </summary>
        /// <value>Indicates whether the user is verified or not.</value>
        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }
    }
}
