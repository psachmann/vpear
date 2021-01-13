// <copyright file="IDiscoveryService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Net;
using System.Threading.Tasks;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Discovers devices for the server.
    /// </summary>
    public interface IDiscoveryService
    {
        /// <summary>
        /// Searches for devices in the given subnet.
        /// </summary>
        /// <param name="address">An address from the subnet to search in.</param>
        /// <param name="subnetMask">The subnet mask from the subnet to search in.</param>
        /// <returns>An asynchronous task.</returns>
        Task SearchDevicesAsync(IPAddress address, IPAddress subnetMask);
    }
}
