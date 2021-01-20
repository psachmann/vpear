// <copyright file="FrameResponse.cs" company="Patrick Sachmann">
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
    public class FrameResponse
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

        /// <summary>
        /// Compares to the given object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>Returns true if objects are equal, false if not.</returns>
        public override bool Equals(object other)
        {
            if (other == null || !(other is FrameResponse))
            {
                return false;
            }

            var frameResponse = (FrameResponse)other;

            if (this.Id == frameResponse.Id
                && this.Time == frameResponse.Time)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the hash code for the object.
        /// </summary>
        /// <returns>Returns the objects hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
