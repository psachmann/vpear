// <copyright file="DeviceFrequencyChangedHandler.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Entities;
using VPEAR.Core.Events;
using VPEAR.Server.Services.Jobs;

namespace VPEAR.Server.Handlers
{
    /// <summary>
    /// The handler for the <see cref="DeviceFrequencyChangedEvent"/> event.
    /// </summary>
    public class DeviceFrequencyChangedHandler : INotificationHandler<DeviceFrequencyChangedEvent>
    {
        private readonly ISchedulerFactory schedulerFactory;
        private readonly ILogger<DeviceFrequencyChangedHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFrequencyChangedHandler"/> class.
        /// </summary>
        /// <param name="schedulerFactory">The scheduler factory.</param>
        /// <param name="logger">The handler logger.</param>
        public DeviceFrequencyChangedHandler(
            ISchedulerFactory schedulerFactory,
            ILogger<DeviceFrequencyChangedHandler> logger)
        {
            this.schedulerFactory = schedulerFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Handel's the <see cref="DeviceFrequencyChangedEvent"/>.
        /// </summary>
        /// <param name="notification">The event notification data.</param>
        /// <param name="cancellationToken">The cancellation token to observe the task while waiting.</param>
        /// <returns>An asynchronous task.</returns>
        public async Task Handle(DeviceFrequencyChangedEvent notification, CancellationToken cancellationToken)
        {
            var device = notification.OriginalValue;

            await this.DeletePollFramesJobAsync(device);

            if (device.Status == DeviceStatus.Recording)
            {
                await this.CreatePollFramesJobAsync(device);
            }
        }

        private static int GetIntervallInSeconds(int frequency)
        {
            var result = 3600 / frequency;

            if (result >= 1)
            {
                return result;
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
            var job = new JobKey($"{device.Id}");

            if (await scheduler.DeleteJob(job))
            {
                this.logger.LogInformation("Deleted job({@Job}) from device({@Device}).", job, device);
            }
        }
    }
}
