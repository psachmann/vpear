// <copyright file="EventDetectorModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.Extensions.Logging;
using Quartz;
using VPEAR.Core.Abstractions;
using VPEAR.Server.Data;
using VPEAR.Server.Data.EventDetectors;

namespace VPEAR.Server
{
    /// <summary>
    /// Encapsulates the Autofac event detector registration in a single module.
    /// </summary>
    public class EventDetectorModule : Module
    {
        /// <summary>
        /// Register all event detectors with the Autofac container.
        /// </summary>
        /// <param name="builder">The container builder to build the Autofac container.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new DeviceStatusChangedEventDetector(
                    context.Resolve<ISchedulerFactory>(),
                    context.Resolve<ILogger<DeviceStatusChangedEventDetector>>()))
                .As<IEventDetector<VPEARDbContext>>()
                .InstancePerRequest();
        }
    }
}
