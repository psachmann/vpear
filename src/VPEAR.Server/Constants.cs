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
            public const string DefaultConfigurationPath = "./appsettings.json";

            public const string DefaultResponseType = "application/json";

            public const int DefaultHttpPort = 80;

            public const int DefaultHttpsPort = 443;
#if DEBUG
            public const LogEventLevel DefaultLogLevel = LogEventLevel.Debug;

            public static readonly List<string> DefaultUrls = new List<string>()
            {
                $"http://localhost:{DefaultHttpPort}/",
            };
#else
            public const LogEventLevel DefaultLogLevel = LogEventLevel.Information;

            public static readonly List<string> DefaultUrls = new List<string>()
            {
                $"http://localhost:{DefaultHttpPort}/",
                $"https://localhost:{DefaultHttpsPort}/",
            };
#endif
        }

        public static class Schemas
        {
            public const string DbSchema = "VPEARDbContext";

            public const string DeviceSchema = "Devices";

            public const string FilterSchema = "Filters";

            public const string FirmwareSchema = "Firmwares";

            public const string FrameSchema = "Frames";

            public const string SensorSchema = "Sensors";

            public const string WifiSchema = "Wifis";
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

            public const int MaxPasswordLength = 1024;

            public const int MinPasswordLength = 8;
        }

        public static class Roles
        {
            public const string AdminRole = "admin";

            public const string UserRole = "user";

            public const string None = "none";

            public static readonly List<string> AllRoles = new List<string>()
            {
                AdminRole,
                UserRole,
            };
        }

        /// <summary>
        /// This class contains all routs for the server.
        /// </summary>
        public static class Routes
        {
            /// <summary>
            /// The prefix for the webapi.
            /// </summary>
            public const string ApiPrefix = "/api/";

            /// <summary>
            /// The version for the webapi.
            /// </summary>
            public const string ApiVersion = "v1/";

            /// <summary>
            /// The base route for all endpoints is the combination
            /// of prefix and version.
            /// </summary>
            public const string BaseRoute = ApiPrefix + ApiVersion;

            /// <summary>
            /// The device endpoint route.
            /// </summary>
            public const string DeviceRoute = BaseRoute + "device";

            /// <summary>
            /// The sensors endpoint route.
            /// </summary>>
            public const string SensorsRoute = BaseRoute + "device/sensors";

            /// <summary>
            /// The frames endpoint route.
            /// </summary>
            public const string FramesRoute = BaseRoute + "device/frames";

            /// <summary>
            /// The filters endpoint route.
            /// </summary>
            public const string FiltersRoute = BaseRoute + "device/filters";

            /// <summary>
            /// The power endpoint route.
            /// </summary>
            public const string PowerRoute = BaseRoute + "device/power";

            /// <summary>
            /// The wifi endpoint route.
            /// </summary>
            public const string WifiRoute = BaseRoute + "device/wifi";

            /// <summary>
            /// The firmware endpoint route.
            /// </summary>
            public const string FirmwareRoute = BaseRoute + "device/firmware";

            public const string UsersRoute = BaseRoute + "user";

            public const string RegisterRoute = BaseRoute + "user/register";

            public const string LoginRoute = BaseRoute + "user/login";

            public const string LogoutRoute = BaseRoute + "user/logout";
        }
    }
}
