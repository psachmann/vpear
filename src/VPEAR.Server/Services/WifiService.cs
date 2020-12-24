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
using static VPEAR.Server.Constants;

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
        /// <param name="wifis">The wifi repository for db access.</param>
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
        public async Task<Result<GetWifiResponse>> GetAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var wifi = await this.wifis.Get()
                .FirstOrDefaultAsync(w => w.DeviceForeignKey.Equals(id));

            if (wifi == null)
            {
                status = HttpStatusCode.NotFound;
                payload = new ErrorResponse(status, ErrorMessages.DeviceNotFound);
            }
            else
            {
                status = HttpStatusCode.OK;
                payload = new GetWifiResponse()
                {
                    Mode = wifi.Mode,
                    Neighbors = wifi.Neighbors,
                    Ssid = wifi.Ssid,
                };
            }

            return new Result<GetWifiResponse>(status, payload);
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutWifiRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var device = await this.devices.GetAsync(id);
            var wifi = await this.wifis.Get()
                .FirstOrDefaultAsync(w => w.DeviceForeignKey.Equals(id));

            if (device == null || wifi == null)
            {
                status = HttpStatusCode.NotFound;
                payload = new ErrorResponse(status, ErrorMessages.DeviceNotFound);
            }
            else if (device.Status == DeviceStatus.Archived)
            {
                status = HttpStatusCode.Gone;
                payload = new ErrorResponse(status, ErrorMessages.DeviceIsArchived);
            }
            else if (device.Status == DeviceStatus.NotReachable)
            {
                status = HttpStatusCode.FailedDependency;
                payload = new ErrorResponse(status, ErrorMessages.DeviceIsNotReachable);
            }
            else if (device.Status == DeviceStatus.Recording || device.Status == DeviceStatus.Stopped)
            {
                // TODO: how to store the password?
                // TODO: synchro service to publish updates to the device
                wifi.Mode = request.Mode;
                wifi.Password = request.Password;
                wifi.Ssid = request.Ssid;

                if (await this.wifis.UpdateAsync(wifi))
                {
                    status = HttpStatusCode.OK;
                    payload = null;
                }
            }

            return new Result<Null>(status, payload);
        }
    }
}
