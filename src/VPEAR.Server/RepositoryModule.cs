// <copyright file="RepositoryModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
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
            builder.RegisterType<Repository<VPEARDbContext, Device, Guid>>()
                .As<IRepository<Device, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Repository<VPEARDbContext, Filter, Guid>>()
                .As<IRepository<Filter, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Repository<VPEARDbContext, Frame, Guid>>()
                .As<IRepository<Frame, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Repository<VPEARDbContext, Sensor, Guid>>()
                .As<IRepository<Sensor, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Repository<VPEARDbContext, Wifi, Guid>>()
                .As<IRepository<Wifi, Guid>>()
                .InstancePerLifetimeScope();
        }
    }
}
