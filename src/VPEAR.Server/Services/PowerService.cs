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
        private readonly ILogger<PowerController> logger;
        private readonly IRepository<Device, Guid> repository;
        private readonly IDeviceClient.Factory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="repository">The device repository for database access.</param>
        /// <param name="factory">The factory for the device client.</param>
        public PowerService(
            ILogger<PowerController> logger,
            IRepository<Device, Guid> repository,
            IDeviceClient.Factory factory)
        {
            this.logger = logger;
            this.repository = repository;
            this.factory = factory;
        }

        /// <inheritdoc/>
        public async Task<Result<GetPowerResponse>> GetAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var device = await this.repository.GetAsync(id);

            if (device == null)
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
                var client = this.factory.Invoke(device.Address);

                status = HttpStatusCode.OK;
                payload = await client.GetPowerAsync();
            }

            return new Result<GetPowerResponse>(status, payload);
        }
    }
}
