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
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Filter, Guid> filters;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<FilterController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterService"/> class.
        /// </summary>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="filters">The filter repository for db access.</param>
        /// <param name="factory">The factory to create a device client.</param>
        /// <param name="logger">The service logger.</param>
        public FilterService(
            IRepository<Device, Guid> devices,
            IRepository<Filter, Guid> filters,
            DeviceClient.Factory factory,
            ILogger<FilterController> logger)
        {
            this.devices = devices;
            this.filters = filters;
            this.factory = factory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<GetFiltersResponse>> GetAsync(Guid id)
        {
            var filter = await this.filters.Get()
                .FirstOrDefaultAsync(f => f.DeviceForeignKey.Equals(id));

            if (filter == null)
            {
                return new Result<GetFiltersResponse>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
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
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutFilterRequest request)
        {
            var device = await this.devices.GetAsync(id);
            var filter = this.filters.Get()
                .FirstOrDefault(f => f.DeviceForeignKey.Equals(id));

            if (device == null || filter == null)
            {
                return new Result<Null>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }

            if (device.Status == DeviceStatus.Archived)
            {
                return new Result<Null>(HttpStatusCode.Gone, ErrorMessages.DeviceIsArchived);
            }

            var client = this.factory.Invoke(device.Address);

            if (device.Status == DeviceStatus.NotReachable || !await client.CanConnectAsync())
            {
                device.Status = DeviceStatus.NotReachable;
                await this.devices.UpdateAsync(device);

                return new Result<Null>(HttpStatusCode.FailedDependency, ErrorMessages.DeviceIsNotReachable);
            }
            else
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

                await this.filters.CreateAsync(filter);
                await this.devices.UpdateAsync(device);

                return new Result<Null>(HttpStatusCode.OK);
            }
        }
    }
}
