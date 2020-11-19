// <copyright file="Wifi.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using VPEAR.Server.Db;

namespace VPEAR.Server.Models
{
    /// <summary>
    /// Db data model for entity framework.
    /// </summary>
    public class Wifi : EntityBase<Guid>
    {
        /// <summary>
        /// Gets or sets the ssid.
        /// </summary>
        /// <value>The ssid for the wifi.</value>
        public string Ssid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode for the wifi.</value>
        public string Mode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>A list of neighboring wifi networks.</value>
        public IList<string> Neighbors { get; set; } = new List<string>();
    }
}
