// <copyright file="DeviceStatusChangedHandler.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Core.Events;
using VPEAR.Server.Services.Jobs;

namespace VPEAR.Server.Handlers
{
    /// <summary>
    /// The handler for the <see cref="DeviceStatusChangedEvent"/> event.
    /// </summary>
    public class DeviceStatusChangedHandler : INotificationHandler<DeviceStatusChangedEvent>
    {
        private readonly ISchedulerFactory schedulerFactory;
        private readonly ILogger<DeviceStatusChangedHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceStatusChangedHandler"/> class.
        /// </summary>
        /// <param name="schedulerFactory">The scheduler factory.</param>
        /// <param name="logger">The handler logger.</param>
        public DeviceStatusChangedHandler(
            ISchedulerFactory schedulerFactory,
            ILogger<DeviceStatusChangedHandler> logger)
        {
            this.schedulerFactory = schedulerFactory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task Handle(DeviceStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            var device = notification.OriginalValue;

            if (device.Status == DeviceStatus.Archived)
            {
                await this.DeletePollFramesJobAsync(device);
            }

            if (device.Status == DeviceStatus.NotReachable)
            {
                await this.DeletePollFramesJobAsync(device);
            }

            if (device.Status == DeviceStatus.Recording)
            {
                await this.DeletePollFramesJobAsync(device);
                await this.CreatePollFramesJobAsync(device);
            }
            else
            {
                await this.DeletePollFramesJobAsync(device);
            }

            if (device.Status == DeviceStatus.Stopped)
            {
                await this.DeletePollFramesJobAsync(device);
            }
        }

        private static int GetIntervallInSeconds(int frequency)
        {
            if (frequency >= 1)
            {
                return frequency;
            }
            else
            {
                return 1;
            }
        }

        private async Task CreatePollFramesJobAsync(Device device)
        {
            var scheduler = await this.schedulerFactory.GetScheduler();
            var job = JobBuilder.Create<PollFramesJob>()
                    .WithIdentity($"{device.Id}")
                    .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{device.Id}", "Poll-Frames-Job")
                .WithSimpleSchedule(builder => builder
                    .WithIntervalInSeconds(GetIntervallInSeconds(device.Frequency))
                    .RepeatForever())
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            this.logger.LogInformation("Created new job({@Job}) with trigger({@Trigger}).", job, trigger);
        }

        private async Task DeletePollFramesJobAsync(Device device)
        {
            var scheduler = await this.schedulerFactory.GetScheduler();
            var job = new JobKey($"{device.Id}-Job");

            if (await scheduler.DeleteJob(job))
            {
                this.logger.LogInformation("Deleted jon({@Job}) from device({@Device}).", job, device);
            }
        }
    }
}
