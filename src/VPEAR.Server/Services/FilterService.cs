// <copyright file="FilterService.cs" company="Patrick Sachmann">
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
    public class FilterService : IFilterService
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<FilterController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterService"/> class.
        /// </summary>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="factory">The factory to create a device client.</param>
        /// <param name="logger">The service logger.</param>
        public FilterService(
            IRepository<Device, Guid> devices,
            DeviceClient.Factory factory,
            ILogger<FilterController> logger)
        {
            this.devices = devices;
            this.factory = factory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<GetFiltersResponse>> GetAsync(Guid id)
        {
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                return new Result<GetFiltersResponse>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }
            else
            {
                await this.devices.GetReferenceAsync(device, device => device.Filter);

                return new Result<GetFiltersResponse>(HttpStatusCode.OK, new GetFiltersResponse()
                {
                    Noise = device.Filter.Noise,
                    Smooth = device.Filter.Smooth,
                    Spot = device.Filter.Noise,
                });
            }
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutFilterRequest request)
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

            if (await client.PutFiltersAsync(request.Spot, request.Smooth, request.Noise))
            {
                await this.devices.GetReferenceAsync(device, device => device.Filter);

                var filter = new Filter()
                {
                    Noise = request.Noise ?? device.Filter.Noise,
                    Smooth = request.Smooth ?? device.Filter.Smooth,
                    Spot = request.Spot ?? device.Filter.Spot,
                };

                device.Filter = filter;

                await this.devices.UpdateAsync(device);

                return new Result<Null>(HttpStatusCode.OK);
            }
            else
            {
                device.Status = DeviceStatus.NotReachable;

                await this.devices.UpdateAsync(device);

                return new Result<Null>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }
        }
    }
}
