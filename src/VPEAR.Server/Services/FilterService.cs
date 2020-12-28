// <copyright file="FilterService.cs" company="Patrick Sachmann">
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
    public class FilterService : IFilterService
    {
        private readonly ILogger<FilterController> logger;
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Filter, Guid> filters;
        private readonly IDeviceClient.Factory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="filters">The filter repository for db access.</param>
        /// <param name="factory">The factory to create a device client.</param>
        public FilterService(
            ILogger<FilterController> logger,
            IRepository<Device, Guid> devices,
            IRepository<Filter, Guid> filters,
            IDeviceClient.Factory factory)
        {
            this.logger = logger;
            this.devices = devices;
            this.filters = filters;
            this.factory = factory;
        }

        /// <inheritdoc/>
        public async Task<Result<GetFiltersResponse>> GetAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var filter = this.filters.Get()
                .FirstOrDefault(f => f.DeviceForeignKey.Equals(id));

            if (filter == null)
            {
                status = HttpStatusCode.NotFound;
                message = ErrorMessages.DeviceNotFound;
            }
            else
            {
                return new Result<GetFiltersResponse>(HttpStatusCode.OK, new GetFiltersResponse()
                {
                    Noise = filter.Noise,
                    Smooth = filter.Smooth,
                    Spot = filter.Noise,
                });
            }

            return new Result<GetFiltersResponse>(status, message);
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutFiltersRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var device = await this.devices.GetAsync(id);
            var filter = this.filters.Get()
                .FirstOrDefault(f => f.DeviceForeignKey.Equals(id));

            if (device == null || filter == null)
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
                    await client.PutFiltersAsync(
                        request.Spot ?? filter.Spot,
                        request.Smooth ?? filter.Smooth,
                        request.Noise ?? filter.Noise);

                    var newFilter = new Filter()
                    {
                        Noise = request.Noise ?? filter.Noise,
                        Smooth = request.Smooth ?? filter.Smooth,
                        Spot = request.Spot ?? filter.Spot,
                    };

                    device.Filters = newFilter;

                    if (await this.filters.CreateAsync(filter)
                        && await this.devices.UpdateAsync(device))
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
