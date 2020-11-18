// <copyright file="FirmwareResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class FirmwareResponse
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The firmware version.</value>
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the upgrade state.
        /// </summary>
        /// <value>The firmware upgrade state.</value>
        [JsonPropertyName("upgrade")]
        public string Upgrade { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The firmaver source channel e.g. stable or unstable.</value>
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
    }
}
