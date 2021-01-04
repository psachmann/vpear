// <copyright file="VPEARDbContextFixture.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using VPEAR.Server.Data;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test
{
    public class VPEARDbContextFixture : IDisposable
    {
        public VPEARDbContextFixture()
        {
            var options = new DbContextOptionsBuilder<VPEARDbContext>()
                .UseInMemoryDatabase(Schemas.DbSchema)
                .Options;

            this.Context = new VPEARDbContext(options);
            this.Seed();
        }

        public VPEARDbContext Context { get; private set; }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        private void Seed()
        {
            this.Context.Database.EnsureDeleted();
            this.Context.Database.EnsureCreated();
        }
    }
}
