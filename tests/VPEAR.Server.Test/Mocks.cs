// <copyright file="Mocks.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Db;

namespace VPEAR.Server.Test
{
    public static class Mocks
    {
        public static readonly Guid NotExistingDeviceId = new Guid("00000000000000000000000000000000");
        public static readonly (Guid Id, DeviceStatus Status) ArchivedDeviceId = (new Guid("00000000000000000000000000000001"), DeviceStatus.Archived);
        public static readonly (Guid Id, DeviceStatus Status) NotReachableDeviceId = (new Guid("00000000000000000000000000000002"), DeviceStatus.NotReachable);
        public static readonly (Guid Id, DeviceStatus Status) RecordingDeviceId = (new Guid("00000000000000000000000000000003"), DeviceStatus.Recording);
        public static readonly (Guid Id, DeviceStatus Status) StoppedDeviceId = (new Guid("00000000000000000000000000000004"), DeviceStatus.Stopped);

        public static ILogger<T> CreateLogger<T>()
        {
            var mock = new Mock<ILogger<T>>();

            return mock.Object;
        }

        public static IRepository<TEntity, Guid> CreateRepository<TEntity>()
            where TEntity : EntityBase<Guid>
        {
            var mock = new Mock<IRepository<TEntity, Guid>>();
            var existingEntities = new List<TEntity>();
            var existingEntityIds = new List<(Guid Id, DeviceStatus Status)>()
            {
                ArchivedDeviceId,
                NotReachableDeviceId,
                RecordingDeviceId,
                StoppedDeviceId,
            };
            var notExistingEntity = Activator.CreateInstance<TEntity>();

            notExistingEntity.Id = NotExistingDeviceId;

            existingEntityIds.ForEach(entityId =>
            {
                var entity = Activator.CreateInstance<TEntity>();
                var info = entity.GetType().GetProperty("DeviceForeignKey");

                if (info != null)
                {
                    entity.Id = entityId.Id;
                    info.SetValue(entity, entityId.Id);
                }
                else if (entity is Device device)
                {
                    device.Id = entityId.Id;
                    device.Status = entityId.Status;
                }

                existingEntities.Add(entity);
            });

            mock.Setup(repository => repository.CreateAsync(It.IsAny<TEntity>()))
                .ReturnsAsync(true);

            mock.Setup(repository => repository.DeleteAsync(notExistingEntity))
                .ReturnsAsync(false);

            existingEntities.ForEach(existingEntity =>
            {
                mock.Setup(repository => repository.DeleteAsync(existingEntity))
                    .ReturnsAsync(true);
            });

            existingEntityIds.ForEach(existingEntityId =>
            {
                var temp = Activator.CreateInstance<TEntity>();
                temp.Id = existingEntityId.Id;

                if (temp is Device device)
                {
                    device.Status = existingEntityId.Status;
                }

                mock.Setup(repository => repository.GetAsync(existingEntityId.Id))
                    .ReturnsAsync(temp);
            });

            mock.Setup(repository => repository.Get())
                .Returns(existingEntities);

            existingEntities.GetRange(0, 2).ForEach(entity =>
            {
                mock.Setup(repository => repository.UpdateAsync(entity))
                    .ReturnsAsync(false);
            });

            existingEntities.GetRange(2, 2).ForEach(entity =>
            {
                mock.Setup(repository => repository.UpdateAsync(entity))
                    .ReturnsAsync(true);
            });

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

        private static IDeviceClient CreateDeviceClient()
        {
            var mock = new Mock<IDeviceClient>();

            mock.Setup(m => m.GetPowerAsync())
                .ReturnsAsync(new GetPowerResponse());

            return mock.Object;
        }
    }
}
