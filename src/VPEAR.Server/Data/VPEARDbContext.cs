// <copyright file="VPEARDbContext.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Entities;
using VPEAR.Server.Data.Configuration;

namespace VPEAR.Server.Data
{
    /// <summary>
    /// The db context for the server.
    /// </summary>
    public class VPEARDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IMediator? mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VPEARDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for the db context.</param>
        /// <param name="mediator">The mediator to dispatch events.</param>
        public VPEARDbContext(DbContextOptions<VPEARDbContext> options, IMediator? mediator)
            : base(options)
        {
            this.mediator = mediator;
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

        public override int SaveChanges()
        {
            return this.SaveChangesAsync()
                .GetAwaiter()
                .GetResult();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (this.mediator == null)
            {
                return result;
            }

            var entitiesWithEvents = this.ChangeTracker.Entries<AbstractEntity<Guid>>()
                .Select(entry => entry.Entity)
                .Where(entity => entity.Events.Any())
                .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                foreach (var abstractEvent in entity.Events.ToArray())
                {
                    await this.mediator.Publish(abstractEvent, cancellationToken)
                        .ConfigureAwait(false);
                }

                entity.Events.Clear();
            }

            return result;
        }

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
