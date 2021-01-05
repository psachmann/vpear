// <copyright file="PollFramesJob.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace VPEAR.Server.Services.Jobs
{
    public class PollFramesJob : IJob
    {
        private readonly ILogger<PollFramesJob> logger;

        public PollFramesJob(ILogger<PollFramesJob> logger)
        {
            this.logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            this.logger.LogInformation("\nPolling frames from device. - {@Time}\n", DateTimeOffset.Now);

            return Task.CompletedTask;
        }
    }
}
