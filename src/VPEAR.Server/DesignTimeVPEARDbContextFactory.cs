// <copyright file="DesignTimeVPEARDbContextFactory.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using VPEAR.Core.Abstractions;
using VPEAR.Server.Data;
using VPEAR.Server.Internals;

namespace VPEAR.Server
{
    public class DesignTimeVPEARDbContextFactory : IDesignTimeDbContextFactory<VPEARDbContext>
    {
        public VPEARDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<VPEARDbContext>();

            Configuration.EnsureLoaded(args);

            builder.UseMySql(
                Startup.Config!.DbConnection,
                new MySqlServerVersion(new Version(Startup.Config.DbVersion)),
                options =>
                {
                    options.CharSetBehavior(CharSetBehavior.NeverAppend);
                });

            return new VPEARDbContext(builder.Options);
        }
    }
}
