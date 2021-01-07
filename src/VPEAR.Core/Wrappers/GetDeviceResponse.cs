// <copyright file="GetDeviceResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class GetDeviceResponse
    {
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("required_sensors")]
        public int ReqioredSensors { get; set; }

        [JsonPropertyName("sample_frequnecy")]
        public double SampleFrequency { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DeviceStatus Status { get; set; }
    }
}
