// <copyright file="ApiResponse.cs" company="Patrick Sachmann">
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
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the device object.
        /// </summary>
        /// <value>The device details.</value>
        [JsonPropertyName("device")]
        public DeviceResponse Device { get; set; } = new DeviceResponse();

        /// <summary>
        /// Gets or sets the sensors object.
        /// </summary>
        /// <value>The sensor details.</value>
        [JsonPropertyName("sensors")]
        public IList<SensorResponse> Sensors { get; set; } = new List<SensorResponse>();

        /// <summary>
        /// Gets or sets the frames object.
        /// </summary>
        /// <value>A list of frames.</value>
        [JsonPropertyName("frames")]
        public IList<FrameResponse> Frames { get; set; } = new List<FrameResponse>();

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The sample frequency.</value>
        [JsonPropertyName("frequency")]
        public uint Frequency { get; set; }

        /// <summary>
        /// Gets or sets the sensors required.
        /// </summary>
        /// <value>The number of required sensors.</value>
        [JsonPropertyName("sensorsRequired")]
        public uint SensorsRequired { get; set; }

        /// <summary>
        /// Gets or sets the filters object.
        /// </summary>
        /// <value>The filter details.</value>
        [JsonPropertyName("filters")]
        public GetFiltersResponse Filters { get; set; } = new GetFiltersResponse();

        /// <summary>
        /// Gets or sets the power object.
        /// </summary>
        /// <value>The power details.</value>
        [JsonPropertyName("power")]
        public GetPowerResponse Power { get; set; } = new GetPowerResponse();

        /// <summary>
        /// Gets or sets the wifi object.
        /// </summary>
        /// <value>The wifi details.</value>
        [JsonPropertyName("wifi")]
        public WifiResponse Wifi { get; set; } = new WifiResponse();

        /// <summary>
        /// Gets or sets the firmware object.
        /// </summary>
        /// <value>The firmware details.</value>
        [JsonPropertyName("firmware")]
        public FirmwareResponse Firmware { get; set; } = new FirmwareResponse();
    }
}
