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
        private readonly ILogger<DeviceController> logger;
        private readonly IRepository<Device, Guid> devices;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceService"/> class.
        /// </summary>
        /// <param name="logger">The service logger.</param>
        /// <param name="devices">The device repository for db access.</param>
        public DeviceService(ILogger<DeviceController> logger, IRepository<Device, Guid> devices)
        {
            this.logger = logger;
            this.devices = devices;
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetDeviceResponse>>> GetAsync(DeviceStatus deviceStatus)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var devices = await this.devices.Get()
                .Where(d => d.Status.Equals(deviceStatus))
                .ToListAsync();

            if (devices == null || devices.Count == 0)
            {
                status = HttpStatusCode.NotFound;
                payload = new ErrorResponse(status, ErrorMessages.DeviceNotFound);
            }
            else
            {
                var container = new Container<GetDeviceResponse>();

                devices.ForEach(device =>
                {
                    container.Items.Add(new GetDeviceResponse()
                    {
                        Address = device.Address,
                        DisplayName = device.DisplayName,
                        Id = device.Id.ToString(),
                        ReqioredSensors = device.RequiredSensors,
                        SampleFrequency = device.SampleFrequency,
                        Status = device.Status,
                    });
                });

                status = HttpStatusCode.OK;
                payload = container;
            }

            return new Result<Container<GetDeviceResponse>>(status, payload);
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(Guid id, PutDeviceRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var device = await this.devices.GetAsync(id);

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
            else
            {
                device.DisplayName = request.DisplayName ?? device.DisplayName;
                device.RequiredSensors = request.RequiredSensors ?? device.RequiredSensors;
                device.SampleFrequency = request.SampleFrequency ?? device.SampleFrequency;

                if (await this.devices.UpdateAsync(device))
                {
                    status = HttpStatusCode.OK;
                    payload = null;
                }
            }

            return new Result<Null>(status, payload);
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
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var device = await this.devices.GetAsync(id);

            if (device == null)
            {
                status = HttpStatusCode.NotFound;
                payload = new ErrorResponse(status, ErrorMessages.DeviceNotFound);
            }
            else if (device.Status == DeviceStatus.Recording)
            {
                status = HttpStatusCode.Conflict;
                payload = new ErrorResponse(status, ErrorMessages.DeviceIsRecording);
            }
            else if (await this.devices.DeleteAsync(device))
            {
                status = HttpStatusCode.OK;
                payload = null;
            }

            return new Result<Null>(status, payload);
        }
    }
}
