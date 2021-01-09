// <copyright file="VPEARDbContext.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VPEAR.Core.Entities;

namespace VPEAR.Server.Data
{
    /// <summary>
    /// The db context for the server.
    /// </summary>
    public class VPEARDbContext : IdentityDbContext<IdentityUser>
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
        public DbSet<Filter>? Filters { get; set; }

        /// <summary>
        /// Gets or sets frames.
        /// </summary>
        /// <value>All frames in the db.</value>
        public DbSet<Frame>? Frames { get; set; }

        /// <summary>
        /// Creates and configures the db model for the entities.
        /// NOTE: Only called by entity framework.
        /// </summary>
        /// <param name="builder">The Entity Framework Core model builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new DeviceConfiguration());
            builder.ApplyConfiguration(new FilterConfiguration());
            builder.ApplyConfiguration(new FrameConfiguration());
        }
    }
}
