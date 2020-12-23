// <copyright file="PostDeviceRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class PostDeviceRequest
    {
        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("start_ip")]
        public string? StartIP { get; set; }

        [JsonPropertyName("stop_ip")]
        public string? StopIP { get; set; }
    }
}
