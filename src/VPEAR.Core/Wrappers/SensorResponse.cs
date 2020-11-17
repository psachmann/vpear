// <copyright file="SensorResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class SensorResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("units")]
        public string Units { get; set; } = string.Empty;

        [JsonPropertyName("columns")]
        public uint Columns { get; set; }

        [JsonPropertyName("rows")]
        public uint Rows { get; set; }

        [JsonPropertyName("width")]
        public uint Width { get; set; }

        [JsonPropertyName("height")]
        public uint Height { get; set; }

        [JsonPropertyName("minimum")]
        public uint Minimum { get; set; }

        [JsonPropertyName("maximum")]
        public uint Maximum { get; set; }
    }
}
