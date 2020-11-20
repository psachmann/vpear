// <copyright file="Firmware.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using VPEAR.Server.Db;

namespace VPEAR.Server.Models
{
    /// <summary>
    /// Db data model for entity framework.
    /// </summary>
    public class Firmware : EntityBase<Guid>
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The firmware version.</value>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the upgrade state.
        /// </summary>
        /// <value>The firmware upgrade state.</value>
        public string Upgrade { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The firmaver source channel e.g. stable or unstable.</value>
        public string Source { get; set; } = string.Empty;
    }
}