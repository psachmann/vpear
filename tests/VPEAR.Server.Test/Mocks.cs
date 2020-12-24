// <copyright file="Mocks.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.Extensions.Logging;
using Moq;
using System;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Db;

namespace VPEAR.Server.Test
{
    public static class Mocks
    {
        public static readonly Guid ArchivedDeviceId = new Guid("00000000000000000000000000000003");
        public static readonly Guid NotExistingDeviceId = new Guid("00000000000000000000000000000000");
        public static readonly Guid NotReachableDeviceId = new Guid("00000000000000000000000000000004");
        public static readonly Guid RecordingDeviceId = new Guid("00000000000000000000000000000002");
        public static readonly Guid StoppedDeviceId = new Guid("00000000000000000000000000000001");

        public static ILogger<T> CreateLogger<T>()
        {
            var mock = new Mock<ILogger<T>>();

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
