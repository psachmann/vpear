// <copyright file="Mocks.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Db;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public static class Mocks
    {
        public const string ConfirmedEmail = "confirmed@email.tld";
        public const string UnconfirmedEmail = "unconfirmed@email.tld";
        public const string ValidPassword = "valid_password";
        public const string InvalidPassword = "invalid_password";
        public static readonly (Guid Id, string Role) Admin = (new Guid("00000000000000000000000000000001"), Roles.AdminRole);
        public static readonly (Guid Id, string Role) User = (new Guid("00000000000000000000000000000002"), Roles.UserRole);
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

        public static IRepository<TEntity, Guid> CreateRepository<TEntity>()
            where TEntity : EntityBase<Guid>
        {
            var mock = new Mock<IRepository<TEntity, Guid>>();
            var notExistingEntity = Activator.CreateInstance<TEntity>();
            var archivedEntity = Activator.CreateInstance<TEntity>();
            var notReachableEntity = Activator.CreateInstance<TEntity>();
            var recordingEntity = Activator.CreateInstance<TEntity>();
            var stoppedEntity = Activator.CreateInstance<TEntity>();

            var entities = new List<TEntity>()
            {
                notExistingEntity,
                archivedEntity,
                notReachableEntity,
                recordingEntity,
                stoppedEntity,
            };

            var ids = new List<(Guid Id, DeviceStatus Status)>()
            {
                NotExisting,
                Archived,
                NotReachable,
                Recording,
                Stopped,
            };

            var i = 0;
            entities.ForEach(entity =>
            {
                var info = entity.GetType()
                    .GetProperty("DeviceForeignKey");

                if (info != null)
                {
                    entity.Id = ids[i].Id;
                    info.SetValue(entity, ids[i].Id);
                }

                if (entity is Device device)
                {
                    device.Id = ids[i].Id;
                    device.Status = ids[i].Status;
                }

                i += 1;
            });

            mock.Setup(repository => repository.CreateAsync(It.IsAny<TEntity>()))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.DeleteAsync(notExistingEntity))
                .ReturnsAsync(false);

            mock.Setup(repository => repository.DeleteAsync(archivedEntity))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.DeleteAsync(notReachableEntity))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.DeleteAsync(recordingEntity))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.DeleteAsync(stoppedEntity))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.GetAsync(Archived.Id))
                    .ReturnsAsync(archivedEntity);

            mock.Setup(repository => repository.GetAsync(NotReachable.Id))
                    .ReturnsAsync(notReachableEntity);

            mock.Setup(repository => repository.GetAsync(Recording.Id))
                    .ReturnsAsync(recordingEntity);

            mock.Setup(repository => repository.GetAsync(Stopped.Id))
                    .ReturnsAsync(stoppedEntity);

            mock.Setup(repository => repository.Get())
                .Returns(new List<TEntity>()
                {
                    archivedEntity,
                    notReachableEntity,
                    recordingEntity,
                    stoppedEntity,
                }.AsQueryable());

            mock.Setup(repository => repository.UpdateAsync(notExistingEntity))
                .ReturnsAsync(false);

            mock.Setup(repository => repository.UpdateAsync(archivedEntity))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.UpdateAsync(notReachableEntity))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.UpdateAsync(recordingEntity))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.UpdateAsync(stoppedEntity))
                .ReturnsAsync(true);

            return mock.Object;
        }

        public static IRepository<TEntity, TKey> CreateRepository<TEntity, TKey>(VPEARDbContext context)
            where TEntity : EntityBase<TKey>
            where TKey : struct, IEquatable<TKey>
        {
            return new Repository<VPEARDbContext, TEntity, TKey>(context, CreateLogger<IRepository<TEntity, TKey>>());
        }

        public static IDeviceClient.Factory CreateDeviceClientFactory()
        {
            var mock = new Mock<IDeviceClient.Factory>();

            mock.Setup(m => m.Invoke(It.IsAny<string>()))
                .Returns(CreateDeviceClient());

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
                .ReturnsAsync(new IdentityResult());

            return mock.Object;
        }

        public static UserManager<IdentityUser> CreateUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var mock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            var admin = new IdentityUser()
            {
                Id = Mocks.Admin.Id.ToString(),
                Email = "admin@mail.tld",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@MAIL.TLD",
                NormalizedUserName = "ADMIN",
                UserName = "Admin",
            };
            var user = new IdentityUser()
            {
                Id = Mocks.User.Id.ToString(),
                Email = "user@email.tld",
                EmailConfirmed = true,
                NormalizedEmail = "USER@MAIL.TLD",
                NormalizedUserName = "USER",
                UserName = "User",
            };

            mock.Object.UserValidators.Add(new UserValidator<IdentityUser>());
            mock.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

            mock.Setup(mock => mock.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(mock => mock.ChangeEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
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
                .ReturnsAsync(new List<string>() { Roles.AdminRole, Roles.UserRole, });

            mock.Setup(mock => mock.GetRolesAsync(user))
                .ReturnsAsync(new List<string>() { Roles.UserRole, });

            mock.Setup(mock => mock.GetUsersInRoleAsync(Roles.AdminRole))
                .ReturnsAsync(new List<IdentityUser>() { admin, });

            mock.Setup(mock => mock.GetUsersInRoleAsync(Roles.UserRole))
                .ReturnsAsync(new List<IdentityUser>() { admin, user, });

            mock.Setup(mock => mock.GetUsersInRoleAsync(Roles.None))
                .ReturnsAsync(new List<IdentityUser>());

            mock.Setup(mock => mock.FindByEmailAsync(ConfirmedEmail))
                .ReturnsAsync(user);

            mock.Setup(mock => mock.FindByIdAsync(admin.Id))
                .ReturnsAsync(admin);

            mock.Setup(mock => mock.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            return mock.Object;
        }

        private static IDeviceClient CreateDeviceClient()
        {
            var mock = new Mock<IDeviceClient>();

            mock.Setup(mock => mock.IsReachableAsync())
                .ReturnsAsync(true);

            mock.Setup(mock => mock.GetPowerAsync())
                .ReturnsAsync(new GetPowerResponse());

            mock.Setup(mock => mock.GetFirmwareAsync())
                .ReturnsAsync(new GetFirmwareResponse());

            return mock.Object;
        }
    }
}
