// <copyright file="DesignTimeVPEARDbContextFactory.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace VPEAR.Server.Data
{
    /// <summary>
    /// Allows Entity Framework Core Tolls to create a <see cref="VPEARDbContext"/> instance during design time.
    /// </summary>
    public class DesignTimeVPEARDbContextFactory : IDesignTimeDbContextFactory<VPEARDbContext>
    {
        /// <summary>
        /// Creates a <see cref="VPEARDbContext"/> instance during design time.
        /// </summary>
        /// <param name="args">The command line args.</param>
        /// <returns>A <see cref="VPEARDbContext"/> instance.</returns>
        public VPEARDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<VPEARDbContext>();

            Server.Configuration.EnsureLoaded(args);

            builder.UseMySql(
                Startup.Config!.DbConnection,
                new MySqlServerVersion(new Version(Startup.Config.DbVersion)),
                options =>
                {
                    options.CharSetBehavior(CharSetBehavior.NeverAppend);
                });

            return new VPEARDbContext(builder.Options, null);
        }
    }
}
