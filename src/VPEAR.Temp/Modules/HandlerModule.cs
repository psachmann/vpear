// <copyright file="HandlerModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Core.Events;
using VPEAR.Server.Handlers;

namespace VPEAR.Server.Modules
{
    /// <summary>
    /// Encapsulates the Autofac client registration in a single module.
    /// </summary>
    public class HandlerModule : Module
    {
        /// <summary>
        /// Register all event handlers with the Autofac container.
        /// </summary>
        /// <param name="builder">The container builder to build the Autofac container.</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(context => new DeviceFrequencyChangedHandler(
                    context.Resolve<ISchedulerFactory>(),
                    context.Resolve<ILogger<DeviceFrequencyChangedHandler>>()))
                .As<INotificationHandler<DeviceFrequencyChangedEvent>>()
                .InstancePerDependency();

            builder.Register(context => new DeviceStatusChangedHandler(
                    context.Resolve<ISchedulerFactory>(),
                    context.Resolve<ILogger<DeviceStatusChangedHandler>>()))
                .As<INotificationHandler<DeviceStatusChangedEvent>>()
                .InstancePerDependency();
        }
    }
}
