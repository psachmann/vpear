// <copyright file="FrameResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class FrameResponse
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The frame id.</value>
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The frame timestamp.</value>
        [JsonPropertyName("time")]
        public string Time { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the readings.
        /// </summary>
        /// <value>The frame sensor values.</value>
        [JsonPropertyName("readings")]
        public ushort[,]? Readings { get; set; }
    }
}
