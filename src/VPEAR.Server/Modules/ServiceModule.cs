// <copyright file="ServiceModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Autofac.Features.OwnedInstances;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Server.Controllers;
using VPEAR.Server.Services;

namespace VPEAR.Server.Modules
{
    /// <summary>
    /// Encapsulates the Autofac service registration in a single module.
    /// </summary>
    public class ServiceModule : Module
    {
        /// <summary>
        /// Register all services with the Autofac container.
        /// </summary>
        /// <param name="builder">The container builder to build the Autofac container.</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(context => new DeviceService(
                    context.Resolve<IRepository<Device, Guid>>(),
                    context.Resolve<DeviceClient.Factory>(),
                    context.Resolve<ISchedulerFactory>(),
                    context.Resolve<ILogger<DeviceController>>()))
                .As<IDeviceService>()
                .InstancePerLifetimeScope();

            builder.Register(context => new FilterService(
                    context.Resolve<IRepository<Device, Guid>>(),
                    context.Resolve<DeviceClient.Factory>(),
                    context.Resolve<ILogger<FilterController>>()))
                .As<IFilterService>()
                .InstancePerLifetimeScope();

            builder.Register(context => new FirmwareService(
                    context.Resolve<IRepository<Device, Guid>>(),
                    context.Resolve<DeviceClient.Factory>(),
                    context.Resolve<ILogger<FirmwareController>>()))
                .As<IFirmwareService>()
                .InstancePerLifetimeScope();

            builder.Register(context => new SensorService(
                    context.Resolve<IRepository<Device, Guid>>(),
                    context.Resolve<IRepository<Frame, Guid>>(),
                    context.Resolve<DeviceClient.Factory>(),
                    context.Resolve<ILogger<SensorController>>()))
                .As<ISensorService>()
                .InstancePerLifetimeScope();

            builder.Register(context => new PowerService(
                    context.Resolve<IRepository<Device, Guid>>(),
                    context.Resolve<DeviceClient.Factory>(),
                    context.Resolve<ILogger<PowerController>>()))
                .As<IPowerService>()
                .InstancePerLifetimeScope();

            builder.Register(context => new UserService(
                    context.Resolve<RoleManager<IdentityRole>>(),
                    context.Resolve<UserManager<IdentityUser>>(),
                    context.Resolve<ILogger<UserController>>()))
                .As<IUserService>()
                .InstancePerLifetimeScope();

            builder.Register(context => new WifiService(
                    context.Resolve<IRepository<Device, Guid>>(),
                    context.Resolve<DeviceClient.Factory>(),
                    context.Resolve<ILogger<WifiController>>()))
                .As<IWifiService>()
                .InstancePerLifetimeScope();
        }
    }
}
