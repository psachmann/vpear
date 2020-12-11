// <copyright file="ConfigurationHelpers.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Internals
{
    internal static class ConfigurationHelpers
    {
        public static Configuration LoadConfiguration(in string[] args)
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

            ValidateConfiguration(config);

            return config;
        }

        private static void ValidateConfiguration(Configuration config)
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
