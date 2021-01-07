// <copyright file="PutDeviceRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class PutDeviceRequest
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The new device display name.</value>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The new device scanning freqency.</value>
        [JsonPropertyName("frequency")]
        public int? Frequency { get; set; }

        /// <summary>
        /// Gets or sets the required sensors.
        /// </summary>
        /// <value>Indicates how many sensors are required for the device.</value>
        [JsonPropertyName("required_sensors")]
        public int? RequiredSensors { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The new device status.</value>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DeviceStatus? Status { get; set; }
    }
}
