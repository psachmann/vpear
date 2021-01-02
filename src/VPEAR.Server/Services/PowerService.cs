// <copyright file="PowerService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

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
    public class PowerService : IPowerService
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<PowerController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerService"/> class.
        /// </summary>
        /// <param name="devices">The device repository for database access.</param>
        /// <param name="factory">The factory for the device client.</param>
        /// <param name="logger">The service logger.</param>
        public PowerService(
            IRepository<Device, Guid> devices,
            DeviceClient.Factory factory,
            ILogger<PowerController> logger)
        {
            this.devices = devices;
            this.factory = factory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<GetPowerResponse>> GetAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var device = await this.devices.GetAsync(id);

            if (device == null)
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
            else if (device.Status == DeviceStatus.Recording || device.Status == DeviceStatus.Stopped)
            {
                var client = this.factory.Invoke(device.Address);

                if (await client.IsReachableAsync())
                {
                    var payload = await client.GetPowerAsync();

                    return new Result<GetPowerResponse>(HttpStatusCode.OK, payload);
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

            return new Result<GetPowerResponse>(status, message);
        }
    }
}
