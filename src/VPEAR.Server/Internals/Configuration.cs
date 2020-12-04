// <copyright file="Configuration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Serilog.Events;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Internals
{
    internal sealed class Configuration
    {
        [JsonPropertyName("database_connection")]
        public string DatabaseConnection { get; set; } = string.Empty;

        [JsonPropertyName("log_level")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogEventLevel LogLevel { get; set; } = Defaults.DefaultLogLevel;

        [JsonPropertyName("http_port")]
        public int HttpPort { get; set; } = Defaults.DefaultHttpPort;

        [JsonPropertyName("https_port")]
        public int HttpsPort { get; set; } = Defaults.DefaultHttpsPort;

        [JsonPropertyName("urls")]
        public List<string> Urls { get; set; } = Defaults.DefaultUrls;
    }
}
