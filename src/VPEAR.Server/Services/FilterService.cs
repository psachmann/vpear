// <copyright file="FilterService.cs" company="Patrick Sachmann">
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
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class FilterService : IFilterService
    {
        private readonly ILogger<FiltersController> logger;
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Filter, Guid> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="filters">The filter repository for db access.</param>
        public FilterService(
            ILogger<FiltersController> logger,
            IRepository<Device, Guid> devices,
            IRepository<Filter, Guid> filters)
        {
            this.logger = logger;
            this.devices = devices;
            this.filters = filters;
        }

        /// <inheritdoc/>
        public async Task<Result<GetFiltersResponse>> GetAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = ErrorMessages.InternalServerError;
            var filter = await this.filters.Get()
                .FirstOrDefaultAsync(f => f.DeviceForeignKey.Equals(id));

            if (filter == null)
            {
                status = HttpStatusCode.NotFound;
                payload = new ErrorResponse(status, ErrorMessages.DeviceNotFound);
            }
            else
            {
                status = HttpStatusCode.OK;
                payload = new GetFiltersResponse()
                {
                    Noise = filter.Noise,
                    Smooth = filter.Smooth,
                    Spot = filter.Noise,
                };
            }

            return new Result<GetFiltersResponse>(status, payload);
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutFiltersRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = ErrorMessages.InternalServerError;
            var device = await this.devices.GetAsync(id);
            var filter = await this.filters.Get()
                .FirstOrDefaultAsync(f => f.DeviceForeignKey.Equals(id));

            if (device == null || filter == null)
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
                // TODO: synchronization service to publish updates to the device
                filter.Noise = request.Noise ?? filter.Noise;
                filter.Smooth = request.Smooth ?? filter.Smooth;
                filter.Spot = request.Spot ?? filter.Spot;

                if (await this.filters.UpdateAsync(filter))
                {
                    status = HttpStatusCode.OK;
                    payload = null;
                }
            }

            return new Result<Null>(status, payload);
        }
    }
}
