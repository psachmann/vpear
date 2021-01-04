// <copyright file="PollFramesJob.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Quartz;
using System;
using System.Threading.Tasks;

namespace VPEAR.Server.Services.Jobs
{
    public class PollFramesJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
