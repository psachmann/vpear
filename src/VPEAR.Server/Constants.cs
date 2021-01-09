// <copyright file="Constants.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Serilog.Events;
using System.Collections.Generic;

namespace VPEAR.Server
{
    internal static class Constants
    {
        public static class Defaults
        {
            public const string DefaultConfigurationPath = "./appsettings.json";
            public const string DefaultResponseType = "application/json";
            public const string DefaultAdminName = "admin";
            public const string DefaultAdminPassword = "Passw0rd?";
            public const string DefaultLogPath = "./logs/log.txt";
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

        public static class WifiModes
        {
            public const string Direct = "direct";
            public const string Indirect = "indirect";
            public static readonly IList<string> All = new List<string>()
            {
                Direct,
                Indirect,
            };
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

        public static class ErrorMessages
        {
            public const string UserNameAlreadyUsed = "The user name is already used.";
            public const string InternalServerError = "An internal server error occurred.";
            public const string LastAdminError = "The last admin can not be deleted.";
            public const string DeviceNotFound = "Device not found.";
            public const string UserNotFound = "User not found.";
            public const string DeviceIsArchivedOrRecording = "Device is archived or currently recording";
            public const string DeviceIsArchived = "Device is archived.";
            public const string DeviceIsNotReachable = "Device is not reachable.";
            public const string BadRequest = "Wrong request format.";
            public const string UserNotVerified = "User not verified.";
            public const string InvalidPassword = "The user password is not correct.";
        }

        public static class Limits
        {
            public const int MaxStringLength = 1024;
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

        public static class Routes
        {
            public const string ApiPrefix = "/api/";
            public const string ApiVersion = "v1/";
            public const string BaseRoute = ApiPrefix + ApiVersion;
            public const string DeviceRoute = BaseRoute + "device";
            public const string SensorsRoute = BaseRoute + "device/sensors";
            public const string FramesRoute = BaseRoute + "device/frames";
            public const string FilterRoute = BaseRoute + "device/filter";
            public const string PowerRoute = BaseRoute + "device/power";
            public const string WifiRoute = BaseRoute + "device/wifi";
            public const string FirmwareRoute = BaseRoute + "device/firmware";
            public const string UsersRoute = BaseRoute + "user";
            public const string RegisterRoute = BaseRoute + "user/register";
            public const string LoginRoute = BaseRoute + "user/login";
            public const string LogoutRoute = BaseRoute + "user/logout";
            public const string TokenRoute = BaseRoute + "user/token";
        }
    }
}
