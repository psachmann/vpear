// <copyright file="Configuration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Internals
{
    internal sealed class Configuration
    {
        [JsonPropertyName("db_connection")]
        public string DbConnection { get; set; } = string.Empty;

        [JsonPropertyName("db_version")]
        public string DbVersion { get; set; } = string.Empty;

        [JsonPropertyName("secret")]
        public string Secret { get; set; } = string.Empty;

        [JsonPropertyName("http_port")]
        public int HttpPort { get; set; } = Defaults.DefaultHttpPort;

        [JsonPropertyName("https_port")]
        public int HttpsPort { get; set; } = Defaults.DefaultHttpsPort;

        [JsonPropertyName("urls")]
        public List<string> Urls { get; set; } = Defaults.DefaultUrls;

        public static void EnsureLoaded(in string[] args)
        {
            if (Startup.Config == null)
            {
                Startup.Config = Configuration.Load(args);
            }
        }

        public static Configuration Load(in string[] args)
        {
            Log.Information("Loading configuration...");

            var path = ExtractPath(in args);
            var json = File.ReadAllText(path, Encoding.UTF8);
            var config = JsonSerializer.Deserialize<Configuration>(json)!;
            config.Urls = new List<string>()
            {
                $"http://localhost:{config.HttpPort}",
                $"https://localhost:{config.HttpsPort}",
            };

            Validate(config);

            return config;
        }

        private static void Validate(Configuration config)
        {
            var validator = new ConfigurationValidator();

            validator.ValidateAndThrow(config);

            Log.Debug("Using configuration \"{@Configuration}\"", config);
        }

        private static string ExtractPath(in string[] args)
        {
            var path = args.Length switch
            {
                2 => args[1],
                _ => Defaults.DefaultConfigurationPath,
            };

            if (string.IsNullOrEmpty(path))
            {
                path = Defaults.DefaultConfigurationPath;
            }

            if (!Path.IsPathRooted(path))
            {
                path = Path.GetFullPath(path);
            }

            Log.Information("Using path \"{Path}\"", path);

            return path;
        }
    }
}
