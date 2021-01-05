// <copyright file="VPEARDbContext.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Models;

namespace VPEAR.Server.Data
{
    /// <summary>
    /// The db context for the server.
    /// </summary>
    public class VPEARDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IEnumerable<IEventDetector<VPEARDbContext>> eventDetectors;

        /// <summary>
        /// Initializes a new instance of the <see cref="VPEARDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for the db context.</param>
        /// <param name="eventDetectors">The event detectors for the db context.</param>
        public VPEARDbContext(DbContextOptions<VPEARDbContext> options, IEnumerable<IEventDetector<VPEARDbContext>> eventDetectors)
            : base(options)
        {
            this.eventDetectors = eventDetectors;
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
        public DbSet<Filter>? Filters { get; set; }

        /// <summary>
        /// Gets or sets firmwares.
        /// </summary>
        /// <value>All firmwares in the db.</value>
        public DbSet<Firmware>? Firmwares { get; set; }

        /// <summary>
        /// Gets or sets frames.
        /// </summary>
        /// <value>All frames in the db.</value>
        public DbSet<Frame>? Frames { get; set; }

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

        /// <inheritdoc/>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var detector in this.eventDetectors)
            {
                await detector.DetectAsync(this);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Creates and configures the db model for the entities.
        /// NOTE: Only called by entity framework.
        /// </summary>
        /// <param name="builder">The ef core model builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new DeviceConfiguration());
            builder.ApplyConfiguration(new FiltersConfiguration());
            builder.ApplyConfiguration(new FirmwareConfiguration());
            builder.ApplyConfiguration(new FrameConfiguration());
            builder.ApplyConfiguration(new SensorConfiguration());
            builder.ApplyConfiguration(new WifiConfiguration());
        }
    }
}
