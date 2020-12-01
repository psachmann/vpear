// <copyright file="PutFiltersRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class PutFiltersRequest
    {
        /// <summary>
        /// Gets or sets a value indicating whether spot is used.
        /// </summary>
        /// <value>Indicates, if the spot filter is used.</value>
        [JsonPropertyName("spot")]
        public bool? Spot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether smooth is used.
        /// </summary>
        /// <value>Indicates, if the smooth filter is used.</value>
        [JsonPropertyName("smooth")]
        public bool? Smooth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether noise is used.
        /// </summary>
        /// <value>Indicates, if the noise filter is used.</value>
        [JsonPropertyName("noise")]
        public bool? Noise { get; set; }
    }
}
