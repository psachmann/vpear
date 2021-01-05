// <copyright file="DeviceStatusChangedEventDetector.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Services.Jobs;

namespace VPEAR.Server.Data.EventDetectors
{
    public class DeviceStatusChangedEventDetector : IEventDetector<VPEARDbContext>
    {
        private readonly ISchedulerFactory factory;
        private readonly ILogger<DeviceStatusChangedEventDetector> logger;

        public DeviceStatusChangedEventDetector(
            ISchedulerFactory factory,
            ILogger<DeviceStatusChangedEventDetector> logger)
        {
            this.factory = factory;
            this.logger = logger;
        }

        public void Detect(VPEARDbContext context)
        {
            throw new System.NotImplementedException();
        }

        public async Task DetectAsync(VPEARDbContext context)
        {
            this.logger.LogDebug("Detecting changes...");

            var name = nameof(Device.Status);
            var changes = context.ChangeTracker.Entries<Device>()
                .Where(device =>
                    device.State == EntityState.Modified
                    && (device.OriginalValues.GetValue<DeviceStatus>(name)
                    != device.CurrentValues.GetValue<DeviceStatus>(name)))
                .ToList();

            this.logger.LogDebug("Detected changes {@Cahnges}", changes);

            var scheduler = await this.factory.GetScheduler();
            foreach (var change in changes)
            {
                // TODO: trigger or terminate poll frames job
                if (change.Entity.Status == DeviceStatus.Recording)
                {
                    var job = JobBuilder.Create<PollFramesJob>()
                        .WithIdentity($"{change.Entity.Id}-Job")
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity($"{change.Entity.Id}-Trigger", "Poll-Frames")
                        .WithSimpleSchedule(builder => builder
                            .WithIntervalInSeconds((int)change.Entity.Frequency)
                            .RepeatForever())
                        .StartNow()
                        .Build();

                    await scheduler.ScheduleJob(job, trigger);

                    this.logger.LogInformation("Created new {@Job} with {@Trigger}", job, trigger);
                }
                else
                {
                    var job = new JobKey($"{change.Entity.Id}-Job");

                    await scheduler.DeleteJob(job);

                    this.logger.LogInformation("Deleted {@Job}", job);
                }

                this.logger.LogInformation("DeviceStatusChangedEvent - {@DeviceStatus}", change.Entity.Status);
            }
        }
    }
}
