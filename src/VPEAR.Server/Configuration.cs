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
using VPEAR.Server.Validators;
using static VPEAR.Server.Constants;

namespace VPEAR.Server
{
    /// <summary>
    /// The vpear configuration class.
    /// </summary>
    public sealed class Configuration
    {
        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>The database connection string used by ef core.</value>
        [JsonPropertyName("db_connection")]
        public string DbConnection { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database version.
        /// </summary>
        /// <value>The MariaDb version e.g. '10.4' or '10.4'.</value>
        [JsonPropertyName("db_version")]
        public string DbVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret that is used for the JWT encryption and decryption. Should be a least 128 characters in length.</value>
        [JsonPropertyName("secret")]
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the http port.
        /// </summary>
        /// <value>The http port used by asp net core.</value>
        [JsonPropertyName("http_port")]
        public int HttpPort { get; set; } = Defaults.DefaultHttpPort;

        /// <summary>
        /// Gets or sets the  https port.
        /// </summary>
        /// <value>The https port used by asp net core.</value>
        [JsonPropertyName("https_port")]
        public int HttpsPort { get; set; } = Defaults.DefaultHttpsPort;

        [JsonIgnore]
        internal List<string> Urls { get; set; } = Defaults.DefaultUrls;

        /// <summary>
        /// Ensures that the configuration was loaded.
        /// </summary>
        /// <param name="args">The program command line args.</param>
        public static void EnsureLoaded(in string[] args)
        {
            if (Startup.Config == null)
            {
                Startup.Config = Configuration.Load(args);
            }
        }

        /// <summary>
        /// Loads the configuration from a default path or the first command line argument, if provided.
        /// </summary>
        /// <param name="args">The program command line args.</param>
        /// <returns>The loaded configuration or null, if the configuration file was not found.</returns>
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
