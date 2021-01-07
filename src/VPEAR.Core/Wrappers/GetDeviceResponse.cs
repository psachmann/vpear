// <copyright file="GetDeviceResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class GetDeviceResponse
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The device address.</value>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The device display name.</value>
        [JsonPropertyName("name")]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        /// <value>The device id.</value>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the required sensors count.
        /// </summary>
        /// <value>The required sensors count.</value>
        [JsonPropertyName("required_sensors")]
        public int RequiredSensors { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The device scanning frequency.</value>
        [JsonPropertyName("frequency")]
        public double Frequency { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The device status.</value>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DeviceStatus Status { get; set; }
    }
}
