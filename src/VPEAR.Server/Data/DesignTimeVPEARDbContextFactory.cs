// <copyright file="DesignTimeVPEARDbContextFactory.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using static VPEAR.Server.Constants;

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
        /// <param name="args">The command line arguments.</param>
        /// <returns>A <see cref="VPEARDbContext"/> instance.</returns>
        public VPEARDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<VPEARDbContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Defaults.DefaultConfigurationPath)
                .Build();
            var connection = configuration.GetValue<string>("MariaDb:Connection");
            var version = new MySqlServerVersion(new Version(configuration.GetValue<string>("MariaDb:Version")));

            builder
                .UseLazyLoadingProxies()
                .UseMySql(connection, version);

            return new VPEARDbContext(builder.Options, null);
        }
    }
}
