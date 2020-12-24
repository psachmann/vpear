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
        private readonly ILogger<FirmwareController> logger;
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Firmware, Guid> firmwares;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirmwareService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="firmwares">The firmware repository for db access.</param>
        public FirmwareService(
            ILogger<FirmwareController> logger,
            IRepository<Device, Guid> devices,
            IRepository<Firmware, Guid> firmwares)
        {
            this.logger = logger;
            this.devices = devices;
            this.firmwares = firmwares;
        }

        /// <inheritdoc/>
        public async Task<Result<GetFirmwareResponse>> GetAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var firmware = await this.firmwares.Get()
                .Where(f => f.DeviceForeignKey.Equals(id))
                .FirstOrDefaultAsync();

            if (firmware == null)
            {
                status = HttpStatusCode.NotFound;
                payload = new ErrorResponse(status, ErrorMessages.DeviceNotFound);
            }
            else
            {
                status = HttpStatusCode.OK;
                payload = new GetFirmwareResponse()
                {
                    Source = firmware.Source,
                    Upgrade = firmware.Upgrade,
                    Version = firmware.Version,
                };
            }

            return new Result<GetFirmwareResponse>(status, payload);
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutFirmwareRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var device = await this.devices.GetAsync(id);
            var firmware = await this.firmwares.Get()
                .Where(f => f.DeviceForeignKey.Equals(id))
                .FirstOrDefaultAsync();

            if (device == null || firmware == null)
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
            else if (request.Package)
            {
                status = HttpStatusCode.NotImplemented;
                payload = new ErrorResponse(status, "Not implemented.");
            }
            else
            {
                firmware.Source = request.Source ?? firmware.Source;
                firmware.Upgrade = request.Upgrade ?? firmware.Upgrade;

                if (await this.firmwares.UpdateAsync(firmware))
                {
                    status = HttpStatusCode.OK;
                    payload = null;
                }
            }

            return new Result<Null>(status, payload);
        }
    }
}
