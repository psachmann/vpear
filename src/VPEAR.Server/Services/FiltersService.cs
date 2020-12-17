// <copyright file="FiltersService.cs" company="Patrick Sachmann">
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
    public class FiltersService : IFiltersService
    {
        private readonly ILogger<FiltersController> logger;
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Filters, Guid> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiltersService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="filters">The filter repository for db access.</param>
        public FiltersService(
            ILogger<FiltersController> logger,
            IRepository<Device, Guid> devices,
            IRepository<Filters, Guid> filters)
        {
            this.logger = logger;
            this.devices = devices;
            this.filters = filters;
        }

        /// <inheritdoc/>
        public async Task<Response> GetAsync(Guid id)
        {
            var filter = await this.filters.Get()
                .FirstOrDefaultAsync(f => f.DeviceForeignKey.Equals(id));

            if (filter == null)
            {
                return new Response(HttpStatusCode.NotFound);
            }

            var payload = new GetFiltersResponse()
            {
                Noise = filter.Noise,
                Smooth = filter.Smooth,
                Spot = filter.Noise,
            };

            return new Response(HttpStatusCode.OK, payload);
        }

        /// <inheritdoc/>
        public async Task<Response> PutAsync(Guid id, PutFiltersRequest request)
        {
            var device = await this.devices.GetAsync(id);
            var filter = await this.filters.Get()
                .FirstOrDefaultAsync(f => f.DeviceForeignKey.Equals(id));

            if (device == null || filter == null)
            {
                return new Response(HttpStatusCode.NotFound);
            }

            if (device.Status == DeviceStatus.Recording || device.Status == DeviceStatus.Stopped)
            {
                // TODO: synchro service to publish updates to the device
                filter.Noise = request.Noise ?? filter.Noise;
                filter.Smooth = request.Smooth ?? filter.Smooth;
                filter.Spot = request.Spot ?? filter.Spot;

                await this.filters.UpdateAsync(filter);

                return new Response(HttpStatusCode.OK);
            }

            if (device.Status == DeviceStatus.Archived)
            {
                return new Response(HttpStatusCode.Gone);
            }

            if (device.Status == DeviceStatus.NotReachable)
            {
                return new Response(HttpStatusCode.FailedDependency);
            }

            return new Response(HttpStatusCode.InternalServerError);
        }
    }
}
