// <copyright file="ApiResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class ApiResponse
    {
        [JsonPropertyName("device")]
        public DeviceResponse Device { get; set; } = new DeviceResponse();

        [JsonPropertyName("sensors")]
        public IList<SensorResponse> Sensors { get; set; } = new List<SensorResponse>();

        [JsonPropertyName("frames")]
        public IList<FrameResponse> Frames { get; set; } = new List<FrameResponse>();

        [JsonPropertyName("frequency")]
        public uint Frequency { get; set; }

        [JsonPropertyName("sensorsRequired")]
        public uint SensorsRequired { get; set; }

        [JsonPropertyName("filters")]
        public FiltersResponse Filters { get; set; } = new FiltersResponse();

        [JsonPropertyName("power")]
        public PowerResponse Power { get; set; } = new PowerResponse();

        [JsonPropertyName("wifi")]
        public WifiResponse Wifi { get; set; } = new WifiResponse();

        [JsonPropertyName("firmware")]
        public FirmwareResponse Firmware { get; set; } = new FirmwareResponse();
    }
}
