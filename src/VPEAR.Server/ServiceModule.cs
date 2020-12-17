// <copyright file="ServiceModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using VPEAR.Core.Abstractions;
using VPEAR.Server.Services;

namespace VPEAR.Server
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
            builder.RegisterType<DeviceService>()
                .As<IDeviceService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FiltersService>()
                .As<IFiltersService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FirmwareService>()
                .As<IFirmwareService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SensorService>()
                .As<ISensorService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PowerService>()
                .As<IPowerService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<WifiService>()
                .As<IWifiService>()
                .InstancePerLifetimeScope();
        }
    }
}
