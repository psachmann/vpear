using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Moq;
using VPEAR.Core.Abstractions;
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

        public static VPEARDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<VPEARDbContext>()
                .UseInMemoryDatabase(Schemas.DbSchema);

            return new VPEARDbContext(builder.Options);
        }

        public static IRepository<TEntity, TKey> CreateRepository<TEntity, TKey>()
            where TEntity : EntityBase<TKey>
            where TKey : struct, IEquatable<TKey>
        {
            return new Repository<VPEARDbContext, TEntity, TKey>(CreateDbContext(),
                CreateLogger<IRepository<TEntity, TKey>>());
        }
    }
}
