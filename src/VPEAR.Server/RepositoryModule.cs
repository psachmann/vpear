// <copyright file="RepositoryModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.Extensions.Logging;
using System;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Server.Data;

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
            base.Load(builder);

            builder.Register(context => new Repository<VPEARDbContext, Device, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Device, Guid>>>()))
                .As<IRepository<Device, Guid>>()
                .InstancePerLifetimeScope();

            builder.Register(context => new Repository<VPEARDbContext, Filter, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Filter, Guid>>>()))
                .As<IRepository<Filter, Guid>>()
                .InstancePerLifetimeScope();

            builder.Register(context => new Repository<VPEARDbContext, Frame, Guid>(
                    context.Resolve<VPEARDbContext>(),
                    context.Resolve<ILogger<IRepository<Frame, Guid>>>()))
                .As<IRepository<Frame, Guid>>()
                .InstancePerLifetimeScope();
        }
    }
}
