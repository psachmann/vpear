// <copyright file="FiltersResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class FiltersResponse
    {
        [JsonPropertyName("spot")]
        public bool Spot { get; set; }

        [JsonPropertyName("smooth")]
        public bool Smooth { get; set; }

        [JsonPropertyName("noise")]
        public bool Noise { get; set; }
    }
}
