// <copyright file="VPEARDbContext.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VPEAR.Server.Models;

namespace VPEAR.Server.Db
{
    /// <summary>
    /// The db context for the server.
    /// </summary>
    public class VPEARDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VPEARDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for the db context.</param>
        public VPEARDbContext(DbContextOptions<VPEARDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets devices.
        /// </summary>
        /// <value>All devices in the db.</value>
        public DbSet<Device>? Devices { get; set; }

        /// <summary>
        /// Gets or sets filters.
        /// </summary>
        /// <value>All filters in the db.</value>
        public DbSet<Filters>? Filters { get; set; }

        /// <summary>
        /// Gets or sets firmwares.
        /// </summary>
        /// <value>All firmewares in the db.</value>
        public DbSet<Firmware>? Firmwares { get; set; }

        /// <summary>
        /// Gets or sets frames.
        /// </summary>
        /// <value>All frames in the db.</value>
        public DbSet<Frame>? Frames { get; set; }

        /// <summary>
        /// Gets or sets powers.
        /// </summary>
        /// <value>All powers in the db.</value>
        public DbSet<Power>? Powers { get; set; }

        /// <summary>
        /// Gets or sets sensors.
        /// </summary>
        /// <value>All sensors in the db.</value>
        public DbSet<Sensor>? Sensors { get; set; }

        /// <summary>
        /// Gets or sets wifis.
        /// </summary>
        /// <value>All wifis in the db.</value>
        public DbSet<Wifi>? Wifis { get; set; }

        /// <summary>
        /// Creates and configures the db model for the entities.
        /// NOTE: Only called by entity framework.
        /// </summary>
        /// <param name="builder">The ef core model builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DeviceConfiguration());
            builder.ApplyConfiguration(new FiltersConfiguration());
            builder.ApplyConfiguration(new FirmwareConfiguration());
            builder.ApplyConfiguration(new FrameConfiguration());
            builder.ApplyConfiguration(new PowerConfiguration());
            builder.ApplyConfiguration(new SensorConfiguration());
            builder.ApplyConfiguration(new WifiConfiguration());
        }
    }
}
