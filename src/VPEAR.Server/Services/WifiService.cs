// <copyright file="WifiService.cs" company="Patrick Sachmann">
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

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class WifiService : IWifiService
    {
        private readonly ILogger<WifiController> logger;
        private readonly IRepository<Device, Guid> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WifiService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="repository">The device repository for database access.</param>
        public WifiService(ILogger<WifiController> logger, IRepository<Device, Guid> repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        /// <inheritdoc/>
        public async Task<Response> GetAsync(Guid id)
        {
            var device = await this.repository.GetAsync(id);

            if (device == null)
            {
                return new Response(HttpStatusCode.NotFound);
            }

            var payload = new GetWifiResponse()
            {
                Mode = device.Wifi.Mode,
                Neighbors = device.Wifi.Neighbors,
                Ssid = device.Wifi.Ssid,
            };

            return new Response(HttpStatusCode.OK, payload);
        }

        /// <inheritdoc/>
        public async Task<Response> PutAsync(Guid id, PutWifiRequest request)
        {
            var device = await this.repository.GetAsync(id);

            if (device == null)
            {
                return new Response(HttpStatusCode.NotFound);
            }

            if (device.Status == DeviceStatus.Active || device.Status == DeviceStatus.Stopped)
            {
                // TODO: how to store the password?
                // TODO: synchro service to publish updates to the device
                device.Wifi = new Wifi()
                {
                    Mode = request.Mode,
                    Neighbors = device.Wifi.Neighbors,
                    Password = request.Password,
                    Ssid = request.Ssid,
                };

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

            return new Response(HttpStatusCode.InternalServerError);
        }
    }
}
