// <copyright file="AutofacFixture.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Services;
using VPEAR.Server.Validators;

namespace VPEAR.Server.Test
{
    public class AutofacFixture : IDisposable
    {
        private static IContainer? root;
        private static bool isInitialized;
        private static object sync = new object();
        private ILifetimeScope? child;

        public AutofacFixture()
        {
            lock (sync)
            {
                if (!isInitialized)
                {
                    var builder = new ContainerBuilder();

                    RegisterClientFactoties(builder);
                    RegisterControllers(builder);
                    RegisterLoggers(builder);
                    RegisterRepositories(builder);
                    RegisterServices(builder);
                    RegisterValidators(builder);

                    root = builder.Build();
                    isInitialized = true;
                }
            }
        }

        public ILifetimeScope Container
        {
            get
            {
                this.child = root!.BeginLifetimeScope();

                return this.child;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.child != null)
            {
                this.child.Dispose();
                this.child = null;
            }
        }

        private static void RegisterClientFactoties(ContainerBuilder builder)
        {
            builder.Register(context => Mocks.CreateDeviceClientFactory())
                .As<DeviceClient.Factory>()
                .InstancePerDependency();
        }

        private static void RegisterControllers(ContainerBuilder builder)
        {
            builder.RegisterType<DeviceController>()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<FilterController>()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<FirmwareController>()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<PowerController>()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<SensorController>()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<UserController>()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<WifiController>()
                .AsSelf()
                .InstancePerDependency();
        }

        private static void RegisterLoggers(ContainerBuilder builder)
        {
            builder.Register(context => Mocks.CreateLogger<DeviceController>())
                .As<ILogger<DeviceController>>()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateLogger<FilterController>())
                .As<ILogger<FilterController>>()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateLogger<FirmwareController>())
                .As<ILogger<FirmwareController>>()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateLogger<PowerController>())
                .As<ILogger<PowerController>>()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateLogger<SensorController>())
                .As<ILogger<SensorController>>()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateLogger<UserController>())
                .As<ILogger<UserController>>()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateLogger<WifiController>())
                .As<ILogger<WifiController>>()
                .InstancePerDependency();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.Register(context => Mocks.CreateDeviceRepository())
                .As<IRepository<Device, Guid>>()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateRepository<Frame>())
                .As<IRepository<Frame, Guid>>()
                .InstancePerDependency();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<DeviceService>()
                .As<IDeviceService>()
                .InstancePerDependency();

            builder.RegisterType<DiscoveryService>()
                .As<IDiscoveryService>()
                .InstancePerDependency();

            builder.RegisterType<FilterService>()
                .As<IFilterService>()
                .InstancePerDependency();

            builder.RegisterType<FirmwareService>()
                .As<IFirmwareService>()
                .InstancePerDependency();

            builder.RegisterType<SensorService>()
                .As<ISensorService>()
                .InstancePerDependency();

            builder.RegisterType<PowerService>()
                .As<IPowerService>()
                .InstancePerDependency();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerDependency();

            builder.RegisterType<WifiService>()
                .As<IWifiService>()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateRoleManager())
                .AsSelf()
                .InstancePerDependency();

            builder.Register(context => Mocks.CreateUserManager())
                .AsSelf()
                .InstancePerDependency();
        }

        private static void RegisterValidators(ContainerBuilder builder)
        {
            builder.RegisterType<PostDeviceValidator>()
                .As<IValidator<PostDeviceRequest>>()
                .InstancePerDependency();

            builder.RegisterType<PostRegisterValidator>()
                .As<IValidator<PostRegisterRequest>>()
                .InstancePerDependency();

            builder.RegisterType<PutDeviceValidator>()
                .As<IValidator<PutDeviceRequest>>()
                .InstancePerDependency();

            builder.RegisterType<PutFilterValidator>()
                .As<IValidator<PutFilterRequest>>()
                .InstancePerDependency();

            builder.RegisterType<PutFirmwareValidator>()
                .As<IValidator<PutFirmwareRequest>>()
                .InstancePerDependency();

            builder.RegisterType<PutLoginValidator>()
                .As<IValidator<PutLoginRequest>>()
                .InstancePerDependency();

            builder.RegisterType<PutUserValidator>()
                .As<IValidator<PutUserRequest>>()
                .InstancePerDependency();

            builder.RegisterType<PutWifiValidator>()
                .As<IValidator<PutWifiRequest>>()
                .InstancePerDependency();
        }
    }
}
