// <copyright file="Constants.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Serilog.Events;
using System.Collections.Generic;


namespace VPEAR.Server
{
    /// <summary>
    /// This class contains all constants for the server.
    /// </summary>
    public partial class Constants
    {
        public static class Defaults
        {
            public const string DefaultConfigurationPath = "../../config/server.config.json";

            public const LogEventLevel DefaultLogLevel = LogEventLevel.Information;

            public const int DefaultHttpPort = 80;

            public const int DefaultHttpsPort = 443;

            public static readonly List<string> DefaultUrls = new List<string>()
            {
                $"http://localhost:{DefaultHttpPort}",
                $"https://localhost:{DefaultHttpsPort}",
            };
        }

        public static class Db
        {
            public const string DefaultSchema = "VPEARDbContext";
        }

        /// <summary>
        /// This class contains specific constrains to prevent magic numbers.
        /// </summary>
        public static class Limits
        {
            /// <summary>
            /// Maximum length for a db string.
            /// </summary>
            public const int MaxStringLength = 1024;

            /// <summary>
            /// Minimum length for a db string.
            /// </summary>
            public const int MinStringLength = 1;
        }

        public static class Roles
        {
            public const string Admin = "admin";

            public const string User = "user";

            public const string Tester = "tester";
        }

        /// <summary>
        /// This class contains all routs for the server.
        /// </summary>
        public static class Routes
        {
            /// <summary>
            /// The prefix for the webapi.
            /// </summary>
            public const string ApiPrefix = "/api";

            /// <summary>
            /// The version for the webapi.
            /// </summary>
            public const string ApiVersion = "/v1";

            /// <summary>
            /// The base route for all endpoints is the combination
            /// of prefix and version.
            /// </summary>
            public const string BaseRoute = ApiPrefix + ApiVersion;

            /// <summary>
            /// The device endpoint route.
            /// </summary>
            public const string DeviceRoute = BaseRoute + "/device";

            /// <summary>
            /// The sensors endpoint route.
            /// </summary>>
            public const string SensorsRoute = BaseRoute + "/device/sensors";

            /// <summary>
            /// The frames endpoint route.
            /// </summary>
            public const string FramesRoute = BaseRoute + "/device/frames";

            /// <summary>
            /// The filters endpoint route.
            /// </summary>
            public const string FiltersRoute = BaseRoute + "/device/filters";

            /// <summary>
            /// The power endpoint route.
            /// </summary>
            public const string PowerRoute = BaseRoute + "/device/power";

            /// <summary>
            /// The wifi endpoint route.
            /// </summary>
            public const string WifiRoute = BaseRoute + "/device/wifi";

            /// <summary>
            /// The firmware endpoint route.
            /// </summary>
            public const string FirmwareRoute = BaseRoute + "/device/firmware";
        }
    }
}
