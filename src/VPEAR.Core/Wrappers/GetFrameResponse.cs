// <copyright file="GetFrameResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class GetFrameResponse
    {
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The frame time stamp.</value>
        [JsonPropertyName("time")]
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Gets or sets the readings.
        /// </summary>
        /// <value>The frame sensor values.</value>
        [JsonPropertyName("readings")]
        public IList<IList<int>> Readings { get; set; } = new List<IList<int>>();

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters applied to the readings.</value>
        [JsonPropertyName("filter")]
        public GetFiltersResponse Filter { get; set; } = new GetFiltersResponse();
    }
}
