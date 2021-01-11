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
using VPEAR.Core.Entities;
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
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                return new Result<GetPowerResponse>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }

            if (device.Status == DeviceStatus.Archived)
            {
                return new Result<GetPowerResponse>(HttpStatusCode.Gone, ErrorMessages.DeviceIsArchived);
            }

            if (device.Status == DeviceStatus.NotReachable)
            {
                return new Result<GetPowerResponse>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }

            var client = this.factory.Invoke(device.Address);

            if (await client.CanConnectAsync())
            {
                var power = await client.GetPowerAsync();
                var payload = new GetPowerResponse()
                {
                    Level = power.Level,
                    State = power.State,
                };

                return new Result<GetPowerResponse>(HttpStatusCode.OK, payload);
            }
            else
            {
                device.StatusChanged(DeviceStatus.NotReachable);

                await this.devices.SaveChangesAsync();

                return new Result<GetPowerResponse>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }
        }
    }
}
