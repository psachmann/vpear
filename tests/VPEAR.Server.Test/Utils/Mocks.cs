// <copyright file="Mocks.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public static class Mocks
    {
        public const string ConfirmedUser = "confirmed_user";
        public const string UnconfirmedUser = "unconfirmed_user";
        public const string ValidPassword = "valid_password";
        public const string InvalidPassword = "invalid_password";
        public static readonly (string Name, string Role) Admin = ("Admin", Roles.AdminRole);
        public static readonly (string Name, string Role) User = ("User", Roles.UserRole);
        public static readonly (Guid Id, DeviceStatus Status) NotExisting = (new Guid("00000000000000000000000000000000"), DeviceStatus.None);
        public static readonly (Guid Id, DeviceStatus Status) Archived = (new Guid("00000000000000000000000000000001"), DeviceStatus.Archived);
        public static readonly (Guid Id, DeviceStatus Status) NotReachable = (new Guid("00000000000000000000000000000002"), DeviceStatus.NotReachable);
        public static readonly (Guid Id, DeviceStatus Status) Recording = (new Guid("00000000000000000000000000000003"), DeviceStatus.Recording);
        public static readonly (Guid Id, DeviceStatus Status) Stopped = (new Guid("00000000000000000000000000000004"), DeviceStatus.Stopped);

        public static ILogger<T> CreateLogger<T>()
        {
            var mock = new Mock<ILogger<T>>();

            return mock.Object;
        }

        public static IRepository<Device, Guid> CreateDeviceRepository()
        {
            var mock = new Mock<IRepository<Device, Guid>>();
            var devices = GetDevices();

            mock.Setup(mock => mock.CreateAsync(It.IsAny<Device>()))
                .ReturnsAsync(It.IsAny<Device>());

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<Device>()))
                .Returns(Task.CompletedTask);

            mock.Setup(mock => mock.Get())
                .Returns(devices.AsQueryable().BuildMock().Object);

            foreach (var device in devices)
            {
                mock.Setup(mock => mock.GetAsync(device.Id))
                    .ReturnsAsync(device);
            }

            mock.Setup(mock => mock.GetReference(It.IsAny<Device>(), It.IsAny<Expression<Func<Device, It.IsAnyType>>>()));

            mock.Setup(mock => mock.GetReferenceAsync(It.IsAny<Device>(), It.IsAny<Expression<Func<Device, It.IsAnyType>>>()))
                .Returns(Task.CompletedTask);

            mock.Setup(mock => mock.GetCollection(It.IsAny<Device>(), It.IsAny<Expression<Func<Device, IEnumerable<It.IsAnyType>>>>()));

            mock.Setup(mock => mock.GetCollectionAsync(It.IsAny<Device>(), It.IsAny<Expression<Func<Device, IEnumerable<It.IsAnyType>>>>()))
                .Returns(Task.CompletedTask);

            mock.Setup(mock => mock.UpdateAsync(It.IsAny<Device>()))
                .ReturnsAsync(It.IsAny<Device>());

            return mock.Object;
        }

        public static IRepository<Frame, Guid> CreateFrameRepository()
        {
            var mock = new Mock<IRepository<Frame, Guid>>();
            var devices = GetDevices();

            mock.Setup(mock => mock.CreateAsync(It.IsAny<Frame>()))
                .ReturnsAsync(It.IsAny<Frame>());

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<Frame>()))
                .Returns(Task.CompletedTask);

            mock.Setup(mock => mock.Get())
                .Returns(devices.Select(device => device.Frames[0])
                    .AsQueryable().BuildMock().Object);

            foreach (var device in devices)
            {
                mock.Setup(mock => mock.GetAsync(device.Id))
                    .ReturnsAsync(device.Frames[0]);
            }

            mock.Setup(mock => mock.GetReference(It.IsAny<Frame>(), It.IsAny<Expression<Func<Frame, It.IsAnyType>>>()));

            mock.Setup(mock => mock.GetReferenceAsync(It.IsAny<Frame>(), It.IsAny<Expression<Func<Frame, It.IsAnyType>>>()))
                .Returns(Task.CompletedTask);

            mock.Setup(mock => mock.GetCollection(It.IsAny<Frame>(), It.IsAny<Expression<Func<Frame, IEnumerable<It.IsAnyType>>>>()));

            mock.Setup(mock => mock.GetCollectionAsync(It.IsAny<Frame>(), It.IsAny<Expression<Func<Frame, IEnumerable<It.IsAnyType>>>>()))
                .Returns(Task.CompletedTask);

            mock.Setup(mock => mock.UpdateAsync(It.IsAny<Frame>()))
                .ReturnsAsync(It.IsAny<Frame>());

            return mock.Object;
        }

        public static DeviceClient.Factory CreateDeviceClientFactory()
        {
            var mock = new Mock<DeviceClient.Factory>();

            mock.Setup(m => m.Invoke(It.IsAny<string>()))
                .Returns(CreateSuccessDeviceClient());

            return mock.Object;
        }

        public static RoleManager<IdentityRole> CreateRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>().Object;
            var roles = new List<IRoleValidator<IdentityRole>>();
            var mock = new Mock<RoleManager<IdentityRole>>(store, roles, new UpperInvariantLookupNormalizer(), new IdentityErrorDescriber(), null);

            roles.Add(new RoleValidator<IdentityRole>());

            mock.Setup(mock => mock.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            mock.Setup(mock => mock.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            return mock.Object;
        }

        public static UserManager<IdentityUser> CreateUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var mock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            var admin = new IdentityUser()
            {
                Email = "admin@mail.tld",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@MAIL.TLD",
                NormalizedUserName = "ADMIN",
                UserName = Admin.Name,
            };
            var user = new IdentityUser()
            {
                Email = "user@email.tld",
                EmailConfirmed = true,
                NormalizedEmail = "USER@MAIL.TLD",
                NormalizedUserName = "USER",
                UserName = User.Name,
            };

            mock.Object.UserValidators.Add(new UserValidator<IdentityUser>());
            mock.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

            mock.Setup(mock => mock.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(mock => mock.ChangePasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(mock => mock.CheckPasswordAsync(It.IsAny<IdentityUser>(), ValidPassword))
                .ReturnsAsync(true);

            mock.Setup(mock => mock.CheckPasswordAsync(It.IsAny<IdentityUser>(), InvalidPassword))
                .ReturnsAsync(false);

            mock.Setup(mock => mock.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(mock => mock.CreateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(mock => mock.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
               .ReturnsAsync(IdentityResult.Success);

            mock.Setup(mock => mock.UpdateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(mock => mock.GetRolesAsync(admin))
                .ReturnsAsync(new List<string>() { Roles.AdminRole, Roles.UserRole });

            mock.Setup(mock => mock.GetRolesAsync(user))
                .ReturnsAsync(new List<string>() { Roles.UserRole });

            mock.Setup(mock => mock.GetUsersInRoleAsync(Roles.AdminRole))
                .ReturnsAsync(new List<IdentityUser>() { admin });

            mock.Setup(mock => mock.GetUsersInRoleAsync(Roles.UserRole))
                .ReturnsAsync(new List<IdentityUser>() { admin, user });

            mock.Setup(mock => mock.GetUsersInRoleAsync(Roles.None))
                .ReturnsAsync(new List<IdentityUser>());

            mock.Setup(mock => mock.FindByNameAsync(ConfirmedUser))
                .ReturnsAsync(user);

            mock.Setup(mock => mock.FindByNameAsync(admin.UserName))
                .ReturnsAsync(admin);

            mock.Setup(mock => mock.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);

            mock.Setup(mock => mock.SetAuthenticationTokenAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            return mock.Object;
        }

        public static IDeviceClient CreateSuccessDeviceClient()
        {
            var mock = new Mock<IDeviceClient>();

            mock.Setup(mock => mock.CanConnectAsync())
                .ReturnsAsync(true);

            mock.Setup(mock => mock.GetAsync())
                .ReturnsAsync(new ApiResponse());

            mock.Setup(mock => mock.GetDeviceAsync())
                .ReturnsAsync(new DeviceResponse());

            mock.Setup(mock => mock.GetFiltersAsync())
                .ReturnsAsync(new FiltersResponse());

            mock.Setup(mock => mock.GetFirmwareAsync())
                .ReturnsAsync(new FirmwareResponse());

            mock.Setup(mock => mock.GetFramesAsync(It.IsAny<int?>()))
                .ReturnsAsync(new List<FrameResponse>());

            mock.Setup(mock => mock.GetFrequencyAsync())
                .ReturnsAsync(60);

            mock.Setup(mock => mock.GetPowerAsync())
                .ReturnsAsync(new PowerResponse());

            mock.Setup(mock => mock.GetRequiredSensorsAsync())
                .ReturnsAsync(1);

            mock.Setup(mock => mock.GetSensorsAsync())
                .ReturnsAsync(new List<SensorResponse>());

            mock.Setup(mock => mock.GetTimeAsync())
                .ReturnsAsync(DateTimeOffset.UtcNow);

            mock.Setup(mock => mock.GetWifiAsync())
                .ReturnsAsync(new WifiResponse());

            mock.Setup(mock => mock.PutFiltersAsync(It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<bool?>()))
                .ReturnsAsync(true);

            mock.Setup(mock => mock.PutFirmwareAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);

            mock.Setup(mock => mock.PutFrequencyAsync(It.IsAny<int?>()))
                .ReturnsAsync(true);

            mock.Setup(mock => mock.PutRequiredSensorsAsync(It.IsAny<int?>()))
                .ReturnsAsync(true);

            mock.Setup(mock => mock.PutTimeAsync(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(true);

            mock.Setup(mock => mock.PutWifiAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            return mock.Object;
        }

        public static IDeviceClient CreateFailureDeviceClient()
        {
            var mock = new Mock<IDeviceClient>();

            mock.Setup(mock => mock.CanConnectAsync())
                .ReturnsAsync(false);

            mock.Setup(mock => mock.PutFiltersAsync(It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<bool?>()))
                .ReturnsAsync(false);

            mock.Setup(mock => mock.PutFirmwareAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(false);

            mock.Setup(mock => mock.PutFrequencyAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            mock.Setup(mock => mock.PutRequiredSensorsAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            mock.Setup(mock => mock.PutTimeAsync(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(false);

            mock.Setup(mock => mock.PutWifiAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            return mock.Object;
        }

        public static ISchedulerFactory CreateSchedulerFactory()
        {
            var mock = new Mock<ISchedulerFactory>();

            mock.Setup(mock => mock.GetScheduler(It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateScheduler());

            mock.Setup(mock => mock.GetScheduler(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateScheduler());

            return mock.Object;
        }

        public static IScheduler CreateScheduler()
        {
            var mock = new Mock<IScheduler>();

            mock.Setup(mock => mock.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(DateTimeOffset.UtcNow);

            mock.Setup(mock => mock.ScheduleJob(It.IsAny<ITrigger>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(DateTimeOffset.UtcNow);

            mock.Setup(mock => mock.DeleteJob(It.IsAny<JobKey>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            return mock.Object;
        }

        public static List<Device> GetDevices()
        {
            var devices = new List<Device>();
            var states = new List<(Guid Id, DeviceStatus Status)>()
            {
                Archived,
                NotReachable,
                Recording,
                Stopped,
            };

            foreach (var (id, status) in states)
            {
                var device = new Device()
                {
                    Address = "http://192.168.178.8",
                    Class = "class",
                    DisplayName = "display_name",
                    Frames = new List<Frame>(),
                    Frequency = 60,
                    Id = id,
                    Name = "name",
                    RequiredSensors = 1,
                    Status = status,
                };

                var filter = new Filter()
                {
                    Device = device,
                    DeviceForeignKey = id,
                    Id = id,
                    Frames = new List<Frame>(),
                    Noise = true,
                    Smooth = true,
                    Spot = true,
                };

                var frame = new Frame()
                {
                    Device = device,
                    DeviceForeignKey = id,
                    Filter = filter,
                    Id = id,
                    Index = 1,
                    Readings = new List<IList<int>>(),
                    Time = "time",
                };

                device.Filter = filter;
                device.Frames.Add(frame);
                filter.Frames.Add(frame);

                devices.Add(device);
            }

            return devices;
        }
    }
}
