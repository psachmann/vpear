// <copyright file="FiltersService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Models;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class FiltersService : IFiltersService
    {
        private readonly ILogger<FiltersController> logger;
        private readonly IRepository<Device, Guid> repository;
        private readonly IDeviceClient.Factory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiltersService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="repository">The device repository for database access.</param>
        /// <param name="factory">The factory for the device client.</param>
        public FiltersService(
            ILogger<FiltersController> logger,
            IRepository<Device, Guid> repository,
            IDeviceClient.Factory factory)
        {
            this.logger = logger;
            this.repository = repository;
            this.factory = factory;
        }

        /// <inheritdoc/>
        public async Task<Response> GetAsync(Guid id)
        {
            var device = await this.repository.GetAsync(id);

            if (device == null)
            {
                return new Response(HttpStatusCode.NotFound);
            }

            var payload = new GetFiltersResponse()
            {
                Noise = device.Filters.Noise,
                Smooth = device.Filters.Smooth,
                Spot = device.Filters.Noise,
            };

            return new Response(HttpStatusCode.OK, payload);
        }

        /// <inheritdoc/>
        public async Task<Response> PutAsync(Guid id, PutFiltersRequest request)
        {
            var device = await this.repository.GetAsync(id);

            if (device == null)
            {
                return new Response(HttpStatusCode.NotFound);
            }

            if (device.Status == DeviceStatus.Active)
            {
                // TODO: synchro service to publish updates to the device
                device.Filters.Noise = request.Noise ?? device.Filters.Noise;
                device.Filters.Smooth = request.Smooth ?? device.Filters.Smooth;
                device.Filters.Spot = request.Spot ?? device.Filters.Spot;

                await this.repository.UpdateAsync(device);

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

            if (device.Status == DeviceStatus.Stopped)
            {
                // TODO: synchro service to publish updates to the device
                device.Filters.Noise = request.Noise ?? device.Filters.Noise;
                device.Filters.Smooth = request.Smooth ?? device.Filters.Smooth;
                device.Filters.Spot = request.Spot ?? device.Filters.Spot;

                await this.repository.UpdateAsync(device);

                return new Response(HttpStatusCode.Accepted);
            }

            return new Response(HttpStatusCode.InternalServerError);
        }
    }
}
