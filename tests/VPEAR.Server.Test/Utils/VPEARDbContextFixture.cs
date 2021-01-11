// <copyright file="VPEARDbContextFixture.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System;
using VPEAR.Server.Data;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public class VPEARDbContextFixture : IDisposable
    {
        private static readonly object Lock = new object();

        public VPEARDbContextFixture()
        {
            Seed();

            this.Context = new VPEARDbContext(GetDbContextOptions(), null);
        }

        public VPEARDbContext Context { get; private set; }

        public void Dispose()
        {
            this.Context.Dispose();
            GC.SuppressFinalize(this);
        }

        private static DbContextOptions<VPEARDbContext> GetDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<VPEARDbContext>()
                .UseInMemoryDatabase(Schemas.DbSchema);

            return builder.Options;
        }

        private static void Seed()
        {
            lock (Lock)
            {
                using var context = new VPEARDbContext(GetDbContextOptions(), null);

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.AddRange(DataSeed.Devices);
                context.SaveChanges();
            }
        }
    }
}
