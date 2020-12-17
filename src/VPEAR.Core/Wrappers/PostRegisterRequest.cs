// <copyright file="PostRegisterRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json request wrapper class.
    /// </summary>
    public class PostRegisterRequest
    {
        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        [Required]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [Required]
        [JsonPropertyName("role")]
        public string? Role { get; set; }
    }
}
