// <copyright file="GetWifiResponse.cs" company="Patrick Sachmann">
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
    public class GetWifiResponse
    {
        /// <summary>
        /// Gets or sets the ssid.
        /// </summary>
        /// <value>The ssid for the wifi.</value>
        [JsonPropertyName("ssid")]
        public string Ssid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode for the wifi.</value>
        [JsonPropertyName("mode")]
        public string Mode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>A list of neighboring wifi networks.</value>
        [JsonPropertyName("list")]
        public IList<string> Neighbors { get; set; } = new List<string>();
    }
}
