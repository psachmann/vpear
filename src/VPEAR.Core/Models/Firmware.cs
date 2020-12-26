// <copyright file="Firmware.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using VPEAR.Core.Abstractions;

namespace VPEAR.Core.Models
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
        /// <value>The firmware source channel e.g. stable or unstable.</value>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the foreign key.
        /// </summary>
        /// <value>The foreign key.</value>
        public Guid DeviceForeignKey { get; set; }

        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>The navigation property.</value>
        public Device? Device { get; set; }
    }
}
