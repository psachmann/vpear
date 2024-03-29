// <copyright file="FrameResponse.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class FrameResponse : IEquatable<FrameResponse>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The frame id.</value>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The frame time stamp.</value>
        [JsonPropertyName("time")]
        public string Time { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the readings.
        /// </summary>
        /// <value>The frame sensor values.</value>
        [JsonPropertyName("readings")]
        public IList<IList<int>> Readings { get; set; } = new List<IList<int>>();

        /// <inheritdoc/>
        public bool Equals(FrameResponse other)
        {
            if (object.ReferenceEquals(this, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Id.Equals(other.Id) && this.Time.Equals(other.Time);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.Time.GetHashCode();
        }
    }
}
