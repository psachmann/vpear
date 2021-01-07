// <copyright file="PutUserRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json request wrapper class.
    /// </summary>
    public class PutUserRequest
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
        /// Gets or sets a value indicating whether the user is verified or not.
        /// </summary>
        /// <value>Indicates whether the user is verified or not.</value>
        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }
    }
}
