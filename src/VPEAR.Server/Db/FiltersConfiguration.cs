// <copyright file="FiltersConfiguration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using VPEAR.Core.Models;

namespace VPEAR.Server.Db
{
    /// <summary>
    /// The entity framework configuration for the <see cref="Filters"/> class.
    /// </summary>
    public class FiltersConfiguration : EntityBaseConfiguration<Filters, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Filters> builder)
        {
            base.Configure(builder);

            builder.HasOne(f => f.Device)
                .WithOne(d => d!.Filters)
                .HasForeignKey<Filters>(f => f.DeviceForeignKey);

            // builder.HasData(DbSeed.Filters);
        }
    }
}
