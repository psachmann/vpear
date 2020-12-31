// <copyright file="DeviceService.cs" company="Patrick Sachmann">
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
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly ILogger<DeviceController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceService"/> class.
        /// </summary>
        /// <param name="devices">The device repository for db access.</param>
        /// <param name="logger">The service logger.</param>
        public DeviceService(IRepository<Device, Guid> devices, ILogger<DeviceController> logger)
        {
            this.devices = devices;
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
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var device = await this.devices.GetAsync(id);

            if (device == null)
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
            else
            {
                device.DisplayName = request.DisplayName ?? device.DisplayName;
                device.RequiredSensors = request.RequiredSensors ?? device.RequiredSensors;
                device.Frequency = request.Frequency ?? device.Frequency;

                if (await this.devices.UpdateAsync(device))
                {
                    return new Result<Null>(HttpStatusCode.OK);
                }
            }

            return new Result<Null>(status, message);
        }

        /// <inheritdoc/>
        public Task<Result<Null>> PostAsync(PostDeviceRequest request)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> DeleteAsync(Guid id)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                status = HttpStatusCode.NotFound;
                message = ErrorMessages.DeviceNotFound;
            }
            else if (device.Status == DeviceStatus.Recording)
            {
                status = HttpStatusCode.Conflict;
                message = ErrorMessages.DeviceIsRecording;
            }
            else if (await this.devices.DeleteAsync(device))
            {
                return new Result<Null>(HttpStatusCode.OK);
            }

            return new Result<Null>(status, message);
        }
    }
}
