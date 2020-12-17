// <copyright file="GetPowerResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class GetPowerResponse
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The power state.</value>
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The power level.</value>
        [JsonPropertyName("level")]
        public uint Level { get; set; }
    }
}
