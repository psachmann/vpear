// <copyright file="Frame.cs" company="Patrick Sachmann">
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
    public class Frame : EntityBase<Guid>
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The frame index.</value>
        public uint Index { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The frame timestamp.</value>
        public string Time { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the readings.
        /// </summary>
        /// <value>The frame sensor values.</value>
        public int[,]? Readings { get; set; }
    }
}
