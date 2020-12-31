// <copyright file="IDiscoveryService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Net;
using System.Threading.Tasks;

namespace VPEAR.Core.Abstractions
{
    public interface IDiscoveryService
    {
        Task SearchDevicesAsync(IPAddress address, IPAddress subnetMask);
    }
}
