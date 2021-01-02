// <copyright file="DeviceResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class DeviceResponse
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The device IP address.</value>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The device class.</value>
        [JsonPropertyName("class")]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The device name.</value>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
