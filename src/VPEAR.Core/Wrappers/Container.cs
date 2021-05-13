// <copyright file="Container.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    /// <typeparam name="TItem">Type of the items.</typeparam>
    public class Container<TItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Container{TItem}"/> class.
        /// </summary>
        public Container()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Container{TItem}"/> class.
        /// </summary>
        /// <param name="start">The start index.</param>
        /// <param name="items">The items to send.</param>
        public Container(int start, IList<TItem> items)
        {
            this.Start = start;
            this.TotalCount = items.Count;
            this.Items = items;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Container{TItem}"/> class.
        /// </summary>
        /// <param name="start">The start index.</param>
        /// <param name="totalCount">Total items count on the server.</param>
        /// <param name="items">The items to send.</param>
        public Container(int start, int totalCount, IList<TItem> items)
        {
            this.Start = start;
            this.TotalCount = totalCount;
            this.Items = items;
        }

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>The start index.</value>
        [JsonPropertyName("start")]
        public int Start { get; set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>Indicates how many items are send.</value>
        [JsonPropertyName("count")]
        public int Count => this.Items.Count;

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>Indicates how many items are on server.</value>
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The requested items.</value>
        [JsonPropertyName("items")]
        public IList<TItem> Items { get; set; } = new List<TItem>();
    }
}
