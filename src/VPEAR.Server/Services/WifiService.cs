// <copyright file="WifiService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class WifiService : IWifiService
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<WifiController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WifiService"/> class.
        /// </summary>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="factory">The factory to create a device client.</param>
        /// <param name="logger">The service logger.</param>
        public WifiService(
            IRepository<Device, Guid> devices,
            DeviceClient.Factory factory,
            ILogger<WifiController> logger)
        {
            this.logger = logger;
            this.devices = devices;
            this.factory = factory;
        }

        /// <inheritdoc/>
        public async Task<Result<GetWifiResponse>> GetAsync(Guid id)
        {
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                return new Result<GetWifiResponse>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }

            if (device.Status == DeviceStatus.Archived)
            {
                return new Result<GetWifiResponse>(HttpStatusCode.Gone, ErrorMessages.DeviceIsArchived);
            }

            if (device.Status == DeviceStatus.NotReachable)
            {
                return new Result<GetWifiResponse>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }

            var client = this.factory.Invoke(device.Address);

            if (await client.CanConnectAsync())
            {
                var wifi = await client.GetWifiAsync();
                var payload = new GetWifiResponse()
                {
                    Mode = wifi.Mode,
                    Neighbors = wifi.Neighbors,
                    Ssid = wifi.Ssid,
                };

                return new Result<GetWifiResponse>(HttpStatusCode.OK, payload);
            }
            else
            {
                device.StatusChanged(DeviceStatus.NotReachable);

                await this.devices.UpdateAsync(device);

                return new Result<GetWifiResponse>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutWifiRequest request)
        {
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                return new Result<Null>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }

            if (device.Status == DeviceStatus.Archived)
            {
                return new Result<Null>(HttpStatusCode.Gone, ErrorMessages.DeviceIsArchived);
            }

            if (device.Status == DeviceStatus.NotReachable)
            {
                return new Result<Null>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }

            var client = this.factory.Invoke(device.Address);

            if (await client.PutWifiAsync(request.Ssid, request.Password, request.Mode))
            {
                await client.SyncAsync(device, this.devices);

                return new Result<Null>(HttpStatusCode.NoContent);
            }
            else
            {
                device.StatusChanged(DeviceStatus.NotReachable);

                await this.devices.UpdateAsync(device);

                return new Result<Null>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }
        }
    }
}
