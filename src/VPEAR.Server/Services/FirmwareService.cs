// <copyright file="FirmwareService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
    public class FirmwareService : IFirmwareService
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Firmware, Guid> firmwares;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<FirmwareController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirmwareService"/> class.
        /// </summary>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="firmwares">The firmware repository for db access.</param>
        /// <param name="factory">The factory to create a device client.</param>
        /// <param name="logger">The service logger.</param>
        public FirmwareService(
            IRepository<Device, Guid> devices,
            IRepository<Firmware, Guid> firmwares,
            DeviceClient.Factory factory,
            ILogger<FirmwareController> logger)
        {
            this.devices = devices;
            this.firmwares = firmwares;
            this.factory = factory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<GetFirmwareResponse>> GetAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var firmware = await this.firmwares.Get()
                .Where(f => f.DeviceForeignKey.Equals(id))
                .FirstOrDefaultAsync();

            if (firmware == null)
            {
                status = HttpStatusCode.NotFound;
                message = ErrorMessages.DeviceNotFound;
            }
            else
            {
                return new Result<GetFirmwareResponse>(HttpStatusCode.OK, new GetFirmwareResponse()
                {
                    Source = firmware.Source,
                    Upgrade = firmware.Upgrade,
                    Version = firmware.Version,
                });
            }

            return new Result<GetFirmwareResponse>(status, message);
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutFirmwareRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var device = await this.devices.GetAsync(id);
            var firmware = this.firmwares.Get()
                .Where(f => f.DeviceForeignKey.Equals(id))
                .FirstOrDefault();

            if (device == null || firmware == null)
            {
                status = HttpStatusCode.NotFound;
                message = ErrorMessages.DeviceNotFound;
            }
            else if (device.Status == DeviceStatus.Archived)
            {
                status = HttpStatusCode.Gone;
                message = ErrorMessages.DeviceIsArchived;
            }
            else if (device.Status == DeviceStatus.NotReachable)
            {
                status = HttpStatusCode.FailedDependency;
                message = ErrorMessages.DeviceIsNotReachable;
            }
            else
            {
                var client = this.factory.Invoke(device.Address);

                if (await client.IsReachableAsync())
                {
                    await client.PutFirmwareAsync(
                        request.Source,
                        request.Upgrade);

                    var response = await client.GetFirmwareAsync();

                    firmware.Source = response!.Source;
                    firmware.Upgrade = response.Upgrade;
                    firmware.Version = response.Version;

                    if (await this.firmwares.UpdateAsync(firmware))
                    {
                        return new Result<Null>(HttpStatusCode.OK);
                    }
                }
                else
                {
                    device.Status = DeviceStatus.NotReachable;

                    if (await this.devices.UpdateAsync(device))
                    {
                        status = HttpStatusCode.FailedDependency;
                        message = ErrorMessages.DeviceIsNotReachable;
                    }
                }
            }

            return new Result<Null>(status, message);
        }
    }
}
