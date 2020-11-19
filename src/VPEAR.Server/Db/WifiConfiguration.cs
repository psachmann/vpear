// <copyright file="WifiConfiguration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using VPEAR.Server.Models;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Db
{
    /// <summary>
    /// The entity framework configuration for the <see cref="Wifi"/> class.
    /// </summary>
    public class WifiConfiguration : EntityBaseConfiguration<Wifi, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Wifi> builder)
        {
            base.Configure(builder);

            builder.Property(w => w.Mode)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.HasMany(w => w.Neighbors)
                .WithOne();

            builder.Property(w => w.Ssid)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();
        }
    }
}
