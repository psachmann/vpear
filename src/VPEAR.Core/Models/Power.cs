// <copyright file="Power.cs" company="Patrick Sachmann">
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
    public class Power : EntityBase<Guid>
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The power state.</value>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The power level.</value>
        public uint Level { get; set; }
    }
}
