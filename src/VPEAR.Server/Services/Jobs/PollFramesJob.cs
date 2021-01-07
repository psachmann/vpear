// <copyright file="PollFramesJob.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;

namespace VPEAR.Server.Services.Jobs
{
    /// <summary>
    /// The job to poll periodically frames from the device.
    /// This job is scheduled by Quartz.
    /// </summary>
    public class PollFramesJob : IJob
    {
        private readonly IRepository<Device, Guid> devices;
        private readonly IRepository<Frame, Guid> frames;
        private readonly DeviceClient.Factory factory;
        private readonly ILogger<PollFramesJob> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PollFramesJob"/> class.
        /// </summary>
        /// <param name="devices">The device repository.</param>
        /// <param name="frames">The frame repository.</param>
        /// <param name="factory">The device client factory.</param>
        /// <param name="logger">The job logger.</param>
        public PollFramesJob(
            IRepository<Device, Guid> devices,
            IRepository<Frame, Guid> frames,
            DeviceClient.Factory factory,
            ILogger<PollFramesJob> logger)
        {
            this.devices = devices;
            this.frames = frames;
            this.factory = factory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task Execute(IJobExecutionContext context)
        {
            var id = new Guid(context.JobDetail.Key.Name);
            var device = await this.devices.GetAsync(id);
            var client = this.factory.Invoke(device.Address);

            if (await client.CanConnectAsync())
            {
                var frames = new List<FrameResponse>();
                var respone = await client.GetFramesAsync();

                await this.devices.GetReferenceAsync(device, device => device.Filter);

                if (respone != null && respone.Count != 0)
                {
                    frames.AddRange(respone);
                    respone = await client.GetFramesAsync(respone[0].Id);
                    frames.AddRange(respone);

                    foreach (var frame in frames)
                    {
                        await this.frames.CreateAsync(new Frame()
                        {
                            Device = device,
                            Filter = device.Filter,
                            Index = frame.Id,
                            Readings = frame.Readings,
                            Time = frame.Time,
                        });
                    }
                }
            }
            else
            {
                await context.Scheduler.DeleteJob(context.JobDetail.Key);

                device.Status = DeviceStatus.NotReachable;

                await this.devices.UpdateAsync(device);

                this.logger.LogError("Client {@ClientId} is not reachable", id);
            }
        }
    }
}
