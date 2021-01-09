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
using VPEAR.Core.Entities;
using VPEAR.Core.Extensions;
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
                device.Frequency = await this.UpdateFrequencyAsync(device, request.Frequency) ?? device.Frequency;
                device.RequiredSensors = request.RequiredSensors ?? device.RequiredSensors;
                device.Status = await this.UpdateStatusAsync(device, request.Status, client) ?? device.Status;

                await this.devices.UpdateAsync(device);
                await client.SyncAsync(device, this.devices);

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

            if (device.Status == DeviceStatus.Archived || device.Status == DeviceStatus.Recording)
            {
                return new Result<Null>(HttpStatusCode.Conflict, ErrorMessages.DeviceIsArchivedOrRecording);
            }

            await this.devices.DeleteAsync(device);

            return new Result<Null>(HttpStatusCode.OK);
        }

        internal static int GetIntervallInSeconds(int frequnecy)
        {
            if (frequnecy >= 1)
            {
                return frequnecy;
            }
            else
            {
                return 1;
            }
        }

        internal async Task DeletePollFramesJobAsync(Device device)
        {
            var scheduler = await this.schedulerFactory.GetScheduler();
            var job = new JobKey(device.Id.ToString());

            await scheduler.DeleteJob(job);

            this.logger.LogInformation("Deleted {@Job}", job);
        }

        internal async Task<int?> UpdateFrequencyAsync(Device device, int? frequency)
        {
            if (frequency == null || frequency == device.Frequency)
            {
                return null;
            }

            await this.DeletePollFramesJobAsync(device);

            if (device.Status == DeviceStatus.Recording)
            {
                var scheduler = await this.schedulerFactory.GetScheduler();
                var job = JobBuilder.Create<PollFramesJob>()
                        .WithIdentity($"{device.Id}-Job")
                        .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"{device.Id}-Trigger", "Poll-Frames")
                    .WithSimpleSchedule(builder => builder
                        .WithIntervalInSeconds(GetIntervallInSeconds(frequency.Value))
                        .RepeatForever())
                    .StartNow()
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                this.logger.LogInformation("Created new {@Job} with {@Trigger}", job, trigger);
            }

            return frequency;
        }

        internal async Task<DeviceStatus?> UpdateStatusAsync(Device device, DeviceStatus? status, IDeviceClient client)
        {
            if (status == null || device.Status == status)
            {
                return null;
            }

            await this.DeletePollFramesJobAsync(device);

            if (device.Status == DeviceStatus.Archived || status == DeviceStatus.Archived)
            {
                return DeviceStatus.Archived;
            }

            if (!await client.CanConnectAsync())
            {
                return DeviceStatus.NotReachable;
            }

            if (status == DeviceStatus.Recording)
            {
                var scheduler = await this.schedulerFactory.GetScheduler();
                var job = JobBuilder.Create<PollFramesJob>()
                        .WithIdentity(device.Id.ToString())
                        .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(device.Id.ToString(), "Poll-Frames")
                    .WithSimpleSchedule(builder => builder
                        .WithIntervalInSeconds(GetIntervallInSeconds(device.Frequency))
                        .RepeatForever())
                    .StartNow()
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                this.logger.LogInformation("Created new {@Job} with {@Trigger}", job, trigger);
            }

            return status;
        }
    }
}
