// <copyright file="PutPasswordRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class PutPasswordRequest
    {
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>The old user password.</value>
        [JsonPropertyName("old_password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>The new user password.</value>
        [JsonPropertyName("new_password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The user token for authentication.</value>
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
