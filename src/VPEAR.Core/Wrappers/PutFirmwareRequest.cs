// <copyright file="PutFirmwareRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class PutFirmwareRequest
    {
        /// <summary>
        /// Gets or sets a value indicating whether to start the upgrade process or not.
        /// </summary>
        /// <value>Indicates whether to start the upgrade process or not.</value>
        [JsonPropertyName("package")]
        public bool Package { get; set; }

        /// <summary>
        /// Gets or sets the upgrade step.
        /// </summary>
        /// <value>Is next or unknown and indicates the upgrade process.</value>
        [JsonPropertyName("upgrade")]
        public string Upgrade { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The device firmware source. Can be stable or unstable.</value>
        [JsonPropertyName("source")]
        public string Source { get; set; }
    }
}
