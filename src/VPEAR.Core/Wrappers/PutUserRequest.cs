// <copyright file="PutUserRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json request wrapper class.
    /// </summary>
    public class PutUserRequest
    {
        [JsonPropertyName("old_password")]
        public string? OldPassword { get; set; }

        [JsonPropertyName("new_password")]
        public string? NewPassword { get; set; }

        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }
    }
}
