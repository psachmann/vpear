// <copyright file="GetSensorResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class GetSensorResponse
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The sensor name.</value>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The sensor units.</value>
        [JsonPropertyName("units")]
        public string Units { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The sensor columns.</value>
        [JsonPropertyName("columns")]
        public uint Columns { get; set; }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The sensor rows.</value>
        [JsonPropertyName("rows")]
        public uint Rows { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The sensor width.</value>
        [JsonPropertyName("width")]
        public uint Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The sensor height.</value>
        [JsonPropertyName("height")]
        public uint Height { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The sensor minimum.</value>
        [JsonPropertyName("minimum")]
        public uint Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The sensor maximum.</value>
        [JsonPropertyName("maximum")]
        public uint Maximum { get; set; }
    }
}
