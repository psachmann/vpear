// <copyright file="JobModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.Extensions.Logging;
using System;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Services.Jobs;

namespace VPEAR.Server
{
    public class JobModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(context => new PollFramesJob(
                    context.Resolve<IRepository<Device, Guid>>(),
                    context.Resolve<IRepository<Frame, Guid>>(),
                    context.Resolve<DeviceClient.Factory>(),
                    context.Resolve<ILogger<PollFramesJob>>()))
                .AsSelf()
                .InstancePerDependency();
        }
    }
}
