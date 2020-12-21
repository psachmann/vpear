// <copyright file="Container.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    public class Container<TItem>
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("start")]
        public long Start { get; set; }

        [JsonPropertyName("stop")]
        public long Stop { get; set; }

        [JsonPropertyName("count")]
        public long Count => this.Items.Count;

        [JsonPropertyName("items")]
        public IList<TItem> Items { get; set; } = new List<TItem>();
    }
}
