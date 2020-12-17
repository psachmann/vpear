using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Moq;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Db;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public static class Mocks
    {
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
