// <copyright file="Constants.Routes.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace VPEAR.Server
{
    /// <summary>
    /// This class contains all constants for the server.
    /// </summary>
    public partial class Constants
    {
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
