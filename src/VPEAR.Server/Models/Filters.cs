// <copyright file="Filters.cs" company="Patrick Sachmann">
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
    public class Filters : EntityBase<Guid>
    {
        /// <summary>
        /// Gets or sets a value indicating whether spot is used.
        /// </summary>
        /// <value>Indicates, if the spot filter is used.</value>
        public bool Spot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether smooth is used.
        /// </summary>
        /// <value>Indicates, if the smooth filter is used.</value>
        public bool Smooth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether noise is used.
        /// </summary>
        /// <value>Indicates, if the noise filter is used.</value>
        public bool Noise { get; set; }
    }
}