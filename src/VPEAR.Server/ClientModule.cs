// <copyright file="ClientModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using VPEAR.Core;
using VPEAR.Core.Abstractions;

namespace VPEAR.Server
{
    /// <summary>
    /// Encapsulates the Autofac client registration in a single module.
    /// </summary>
    public class ClientModule : Module
    {
        /// <summary>
        /// Register all clients with the Autofac container.
        /// </summary>
        /// <param name="builder">The container builder to build the Autofac container.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DeviceClient>()
                .As<IDeviceClient>()
                .InstancePerLifetimeScope();
        }
    }
}
