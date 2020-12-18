// <copyright file="FiltersConfiguration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using VPEAR.Core.Models;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Db
{
    /// <summary>
    /// The entity framework configuration for the <see cref="Filter"/> class.
    /// </summary>
    public class FiltersConfiguration : EntityBaseConfiguration<Filter, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Filter> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.FilterSchema);

            builder.HasOne(f => f.Device)
                .WithOne(d => d.Filters)
                .HasForeignKey<Filter>(f => f.DeviceForeignKey);
#if DEBUG
            builder.HasData(DbSeed.Filters);
#endif
        }
    }
}
