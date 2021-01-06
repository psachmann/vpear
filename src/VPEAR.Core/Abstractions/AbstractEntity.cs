// <copyright file="AbstractEntity.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Base class for all db entities.
    /// NOTE: All db entities should derive from this class.
    /// </summary>
    /// <typeparam name="TKey">Type of the db id.</typeparam>
    public abstract class AbstractEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the creation date time.
        /// NOTE: Auto generated by ef core.
        /// </summary>
        /// <value>The date time, when the specific item was created.</value>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the unique id.
        /// NOTE: Auto generated by ef core.
        /// </summary>
        /// <value>The unique id, which is the key in the database.</value>
        public TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the concurrency token.
        /// NOTE: Auto generated by ef core.
        /// </summary>
        /// <value>The concurrency token for the specific item.</value>
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
