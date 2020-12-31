// <copyright file="RepositoryModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.Extensions.Logging;
using System;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Server.Db;

namespace VPEAR.Server
{
    /// <summary>
    /// Encapsulates the Autofac repository registration in a single module.
    /// </summary>
    public class RepositoryModule : Module
    {
        /// <summary>
        /// Register all repositories with the Autofac container.
        /// </summary>
        /// <param name="builder">The container builder to build the Autofac container.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new Repository<VPEARDbContext, Device, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Device, Guid>>>()))
                .As<IRepository<Device, Guid>>()
                .InstancePerRequest();

            builder.Register(context => new Repository<VPEARDbContext, Filter, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Filter, Guid>>>()))
                .As<IRepository<Filter, Guid>>()
                .InstancePerRequest();

            builder.Register(context => new Repository<VPEARDbContext, Firmware, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Firmware, Guid>>>()))
                .As<IRepository<Firmware, Guid>>()
                .InstancePerRequest();

            builder.Register(context => new Repository<VPEARDbContext, Frame, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Frame, Guid>>>()))
                .As<IRepository<Frame, Guid>>()
                .InstancePerRequest();

            builder.Register(context => new Repository<VPEARDbContext, Sensor, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Sensor, Guid>>>()))
                .As<IRepository<Sensor, Guid>>()
                .InstancePerRequest();

            builder.Register(context => new Repository<VPEARDbContext, Wifi, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Wifi, Guid>>>()))
                .As<IRepository<Wifi, Guid>>()
                .InstancePerRequest();
        }
    }
}
