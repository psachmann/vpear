// <copyright file="FilterConfiguration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using VPEAR.Core.Models;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Data
{
    /// <summary>
    /// The entity framework configuration for the <see cref="Filter"/> class.
    /// </summary>
    public class FilterConfiguration : AbstractEntityConfiguration<Filter, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Filter> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.FilterSchema);

            builder.HasOne(filter => filter.Device)
                .WithOne(device => device.Filter)
                .HasForeignKey<Filter>(filter => filter.DeviceForeignKey);

            builder.HasMany(filter => filter.Frames)
                .WithOne(frame => frame.Filter);
#if DEBUG
            builder.HasData(SeedData.Filters);
#endif
        }
    }
}
