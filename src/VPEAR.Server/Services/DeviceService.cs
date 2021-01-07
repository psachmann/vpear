// <copyright file="DeviceService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
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
using VPEAR.Server.Services.Jobs;
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
        private readonly DeviceClient.Factory factory;
        private readonly ISchedulerFactory schedulerFactory;
        private readonly ILogger<DeviceController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceService"/> class.
        /// </summary>
        /// <param name="devices">The device repository.</param>
        /// <param name="discovery">The discovery service.</param>
        /// <param name="factory">The device client factory.</param>
        /// <param name="schedulerFactory">The Quartz scheduler factory.</param>
        /// <param name="logger">The service logger.</param>
        public DeviceService(
            IRepository<Device, Guid> devices,
            IDiscoveryService discovery,
            DeviceClient.Factory factory,
            ISchedulerFactory schedulerFactory,
            ILogger<DeviceController> logger)
        {
            this.devices = devices;
            this.discovery = discovery;
            this.factory = factory;
            this.schedulerFactory = schedulerFactory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetDeviceResponse>>> GetAsync(DeviceStatus? deviceStatus)
        {
            var devices = deviceStatus switch
            {
                null => await this.devices.Get()
                    .ToListAsync(),
                _ => await this.devices.Get()
                    .Where(d => d.Status.Equals(deviceStatus))
                    .ToListAsync(),
            };

            if (devices.Count == 0)
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
                        RequiredSensors = device.RequiredSensors,
                        Frequency = device.Frequency,
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

            var client = this.factory.Invoke(device.Address);

            if (await client.PutFrequencyAsync(request.Frequency) && await client.PutRequiredSensorsAsync(request.RequiredSensors))
            {
                device.DisplayName = request.DisplayName ?? device.DisplayName;
                device.Frequency = await this.UpdatePollFramesJobAsync(device, request.Frequency) ?? device.Frequency;
                device.RequiredSensors = request.RequiredSensors ?? device.RequiredSensors;
                device.Status = await this.CreatePollFramesJobAsync(device, request.Status) ?? device.Status;

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

        /// <inheritdoc/>
        public async Task<Result<Null>> PostAsync(PostDeviceRequest request)
        {
            var address = IPAddress.Parse(request.Address!);
            var subnetMask = IPAddress.Parse(request.SubnetMask!);

            if (address.IsIPv4() && subnetMask.IsIPv4() && subnetMask.IsIPv4SubnetMask())
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

        private async Task<DeviceStatus?> CreatePollFramesJobAsync(Device device, DeviceStatus? status)
        {
            if (status == null)
            {
                return status;
            }

            var scheduler = await this.schedulerFactory.GetScheduler();

            if (device.Status == DeviceStatus.Stopped && status == DeviceStatus.Recording)
            {
                var job = JobBuilder.Create<PollFramesJob>()
                        .WithIdentity(device.Id.ToString())
                        .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(device.Id.ToString(), "Poll-Frames")
                    .WithSimpleSchedule(builder => builder
                        .WithIntervalInSeconds(device.Frequency)
                        .RepeatForever())
                    .StartNow()
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                this.logger.LogInformation("Created new {@Job} with {@Trigger}", job, trigger);
            }
            else
            {
                var job = new JobKey(device.Id.ToString());

                await scheduler.DeleteJob(job);

                this.logger.LogInformation("Deleted {@Job}", job);
            }

            return status;
        }

        private async Task<int?> UpdatePollFramesJobAsync(Device device, int? frequency)
        {
            if (frequency == null)
            {
                return frequency;
            }

            var scheduler = await this.schedulerFactory.GetScheduler();

            if (device.Status == DeviceStatus.Recording && device.Frequency != frequency)
            {
                var jobKey = new JobKey($"{device.Id}-Job");

                await scheduler.DeleteJob(jobKey);

                this.logger.LogInformation("Deleted {@Job}", jobKey);

                var job = JobBuilder.Create<PollFramesJob>()
                        .WithIdentity($"{device.Id}-Job")
                        .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"{device.Id}-Trigger", "Poll-Frames")
                    .WithSimpleSchedule(builder => builder
                        .WithIntervalInSeconds(frequency.Value)
                        .RepeatForever())
                    .StartNow()
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                this.logger.LogInformation("Created new {@Job} with {@Trigger}", job, trigger);
            }

            return frequency;
        }
    }
}
