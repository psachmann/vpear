// <copyright file="Wifi.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using VPEAR.Core.Abstractions;

namespace VPEAR.Core.Models
{
    /// <summary>
    /// Db data model for entity framework.
    /// </summary>
    public class Wifi : AbstractEntity<Guid>
    {
        /// <summary>
        /// Gets or sets the ssid.
        /// </summary>
        /// <value>The wifi ssid.</value>
        public string Ssid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The wifi mode.</value>
        public string Mode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>A list of neighboring wifi networks.</value>
        public IList<string> Neighbors { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the foreign key.
        /// </summary>
        /// <value>The foreign key.</value>
        public Guid DeviceForeignKey { get; set; }

        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>The navigation property.</value>
        public Device Device { get; set; }
    }
}
