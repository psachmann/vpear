// <copyright file="WifiService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class WifiService : IWifiService
    {
        private readonly ILogger<WifiController> logger;
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Wifi, Guid> wifis;

        /// <summary>
        /// Initializes a new instance of the <see cref="WifiService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="wifis">The wifi repositroy for db access.</param>
        public WifiService(
            ILogger<WifiController> logger,
            IRepository<Device, Guid> devices,
            IRepository<Wifi, Guid> wifis)
        {
            this.logger = logger;
            this.devices = devices;
            this.wifis = wifis;
        }

        /// <inheritdoc/>
        public async Task<Result> GetAsync(Guid id)
        {
            var wifi = await this.wifis.Get()
                .FirstOrDefaultAsync(w => w.DeviceForeignKey.Equals(id));

            if (wifi == null)
            {
                return new Result(HttpStatusCode.NotFound);
            }

            var payload = new GetWifiResponse()
            {
                Mode = wifi.Mode,
                Neighbors = wifi.Neighbors,
                Ssid = wifi.Ssid,
            };

            return new Result(HttpStatusCode.OK, payload);
        }

        /// <inheritdoc/>
        public async Task<Result> PutAsync(Guid id, PutWifiRequest request)
        {
            var device = await this.devices.GetAsync(id);
            var wifi = await this.wifis.Get()
                .FirstOrDefaultAsync(w => w.DeviceForeignKey.Equals(id));

            if (device == null || wifi == null)
            {
                return new Result(HttpStatusCode.NotFound);
            }

            if (device.Status == DeviceStatus.Recording || device.Status == DeviceStatus.Stopped)
            {
                // TODO: how to store the password?
                // TODO: synchro service to publish updates to the device
                wifi.Mode = request.Mode;
                wifi.Password = request.Password;
                wifi.Ssid = request.Ssid;

                await this.wifis.UpdateAsync(wifi);

                return new Result(HttpStatusCode.OK);
            }

            if (device.Status == DeviceStatus.Archived)
            {
                return new Result(HttpStatusCode.Gone);
            }

            if (device.Status == DeviceStatus.NotReachable)
            {
                return new Result(HttpStatusCode.FailedDependency);
            }

            return new Result(HttpStatusCode.InternalServerError);
        }
    }
}
