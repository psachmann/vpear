// <copyright file="GetSensorsResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class GetSensorsResponse
    {
        [JsonPropertyName("sensors")]
        public IList<SensorResponse> Sensors { get; set; } = new List<SensorResponse>();
    }
}
