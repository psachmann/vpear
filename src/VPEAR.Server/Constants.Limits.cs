// <copyright file="Constants.Limits.cs" company="Patrick Sachmann">
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
    }
}
