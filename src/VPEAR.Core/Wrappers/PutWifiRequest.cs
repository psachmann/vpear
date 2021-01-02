// <copyright file="PutWifiRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class PutWifiRequest
    {
        /// <summary>
        /// Gets or sets the ssid.
        /// </summary>
        /// <value>The network to connect to.</value>
        [JsonPropertyName("ssid")]
        public string Ssid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The network password.</value>
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The device wifi mode.</value>
        [JsonPropertyName("mode")]
        public string Mode { get; set; } = string.Empty;
    }
}
