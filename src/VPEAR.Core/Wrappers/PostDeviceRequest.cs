// <copyright file="PostDeviceRequest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization;

namespace VPEAR.Core.Wrappers
{
    /// <summary>
    /// A json wrapper class with json naming conventions.
    /// </summary>
    public class PostDeviceRequest
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>A IP address from the subnet to search.</value>
        [JsonPropertyName("address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the subnet mask.
        /// </summary>
        /// <value>The subnet mask form the subnet to search.</value>
        [JsonPropertyName("subent_mask")]
        public string SubnetMask { get; set; }
    }
}
