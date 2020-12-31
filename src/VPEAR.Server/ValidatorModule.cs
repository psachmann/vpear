// <copyright file="ValidatorModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Validators;

namespace VPEAR.Server
{
    /// <summary>
    /// Encapsulates the Autofac validator registration in a single module.
    /// </summary>
    public class ValidatorModule : Module
    {
        /// <summary>
        /// Register all validators with the Autofac container.
        /// </summary>
        /// <param name="builder">The container builder to build the Autofac container.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new PostDeviceValidator())
                .As<IValidator<PostDeviceRequest>>()
                .InstancePerRequest();

            builder.Register(context => new PostRegisterValidator())
                .As<IValidator<PostRegisterRequest>>()
                .InstancePerRequest();

            builder.Register(context => new PutDeviceValidator())
                .As<IValidator<PutDeviceRequest>>()
                .InstancePerRequest();

            builder.Register(context => new PutFilterValidator())
                .As<IValidator<PutFilterRequest>>()
                .InstancePerRequest();

            builder.Register(context => new PutFirmwareValidator())
                .As<IValidator<PutFirmwareRequest>>()
                .InstancePerRequest();

            builder.Register(context => new PutLoginValidator())
                .As<IValidator<PutLoginRequest>>()
                .InstancePerRequest();

            builder.Register(context => new PutUserValidator())
                .As<IValidator<PutUserRequest>>()
                .InstancePerRequest();

            builder.Register(context => new PutWifiValidator())
                .As<IValidator<PutWifiRequest>>()
                .InstancePerRequest();
        }
    }
}
