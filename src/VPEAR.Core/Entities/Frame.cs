// <copyright file="Frame.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using VPEAR.Core.Abstractions;

namespace VPEAR.Core.Entities
{
    /// <summary>
    /// Db data model for entity framework.
    /// </summary>
    public class Frame : AbstractEntity<Guid>, IEquatable<Frame>
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The frame index.</value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The frame time stamp.</value>
        public string Time { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the readings.
        /// </summary>
        /// <value>The frame sensor values.</value>
        public virtual IList<IList<int>> Readings { get; set; } = new List<IList<int>>();

        /// <summary>
        /// Gets or sets the foreign key.
        /// </summary>
        /// <value>The device foreign key.</value>
        public Guid DeviceForeignKey { get; set; }

        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>The device navigation property.</value>
        public virtual Device Device { get; set; }

        /// <summary>
        /// Gets or sets the foreign key.
        /// </summary>
        /// <value>The Filter foreign key.</value>
        public Guid FilterForeignKey { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter navigation property.</value>
        public virtual Filter Filter { get; set; }

        /// <inheritdoc/>
        public bool Equals(Frame other)
        {
            if (object.ReferenceEquals(this, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Index.Equals(other.Index) && this.Time.Equals(other.Time);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Index.GetHashCode() ^ this.Time.GetHashCode();
        }
    }
}
