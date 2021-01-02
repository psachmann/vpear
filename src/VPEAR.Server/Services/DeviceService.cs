// <copyright file="DeviceService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Extensions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IDeviceService"/> interface.
    /// </summary>
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly IDiscoveryService discovery;
        private readonly ILogger<DeviceController> logger;

        public DeviceService(
            IRepository<Device, Guid> devices,
            IDiscoveryService discovery,
            ILogger<DeviceController> logger)
        {
            this.devices = devices;
            this.discovery = discovery;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetDeviceResponse>>> GetAsync(DeviceStatus deviceStatus)
        {
            var devices = await this.devices.Get()
                .Where(d => d.Status.Equals(deviceStatus))
                .ToListAsync();

            if (devices == null || devices.Count == 0)
            {
                return new Result<Container<GetDeviceResponse>>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }
            else
            {
                var payload = new Container<GetDeviceResponse>();

                devices.ForEach(device =>
                {
                    payload.Items.Add(new GetDeviceResponse()
                    {
                        Address = device.Address,
                        DisplayName = device.DisplayName,
                        Id = device.Id.ToString(),
                        ReqioredSensors = device.RequiredSensors,
                        SampleFrequency = device.Frequency,
                        Status = device.Status,
                    });
                });

                return new Result<Container<GetDeviceResponse>>(HttpStatusCode.OK, payload);
            }
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutDeviceRequest request)
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

            device.DisplayName = request.DisplayName ?? device.DisplayName;
            device.RequiredSensors = request.RequiredSensors ?? device.RequiredSensors;
            device.Frequency = request.Frequency ?? device.Frequency;

            await this.devices.UpdateAsync(device);

            return new Result<Null>(HttpStatusCode.OK);
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PostAsync(PostDeviceRequest request)
        {
            var address = IPAddress.Parse(request.Address!);
            var subnetMask = IPAddress.Parse(request.SubnetMask!);

            if (address.IsIPv4() && subnetMask.IsIPv4())
            {
                await this.discovery.SearchDevicesAsync(address, subnetMask);

                return new Result<Null>(HttpStatusCode.Processing);
            }
            else
            {
                return new Result<Null>(HttpStatusCode.BadRequest, ErrorMessages.BadRequest);
            }
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> DeleteAsync(Guid id)
        {
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                return new Result<Null>(HttpStatusCode.NotFound, ErrorMessages.DeviceNotFound);
            }

            if (device.Status == DeviceStatus.Recording)
            {
                return new Result<Null>(HttpStatusCode.Conflict, ErrorMessages.DeviceIsRecording);
            }

            await this.devices.DeleteAsync(device);

            return new Result<Null>(HttpStatusCode.OK);
        }
    }
}
