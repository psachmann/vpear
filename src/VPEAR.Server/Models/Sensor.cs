// <copyright file="Sensor.cs" company="Patrick Sachmann">
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
    public class Sensor : EntityBase<Guid>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The sensor name.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The sensor units.</value>
        public string Units { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The sensor columns.</value>
        public uint Columns { get; set; }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The sensor rows.</value>
        public uint Rows { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The sensor width.</value>
        public uint Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The sensor height.</value>
        public uint Height { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The sensor minimum.</value>
        public uint Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The sensor maximum.</value>
        public uint Maximum { get; set; }
    }
}
