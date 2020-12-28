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
                .InstancePerRequest();

            builder.RegisterType<FilterService>()
                .As<IFilterService>()
                .InstancePerRequest();

            builder.RegisterType<FirmwareService>()
                .As<IFirmwareService>()
                .InstancePerRequest();

            builder.RegisterType<SensorService>()
                .As<ISensorService>()
                .InstancePerRequest();

            builder.RegisterType<PowerService>()
                .As<IPowerService>()
                .InstancePerRequest();

            builder.RegisterType<WifiService>()
                .As<IWifiService>()
                .InstancePerRequest();
        }
    }
}
