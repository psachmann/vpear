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
        /// Gets or sets the start index.
        /// </summary>
        /// <value>The start index.</value>
        [JsonPropertyName("start")]
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>Indicates how many items are on server.</value>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The requested items.</value>
        [JsonPropertyName("items")]
        public IList<TItem> Items { get; set; } = new List<TItem>();
    }
}
