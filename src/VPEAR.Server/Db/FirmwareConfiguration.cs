// <copyright file="FirmwareConfiguration.cs" company="Patrick Sachmann">
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
    /// The entity framework configuration for the <see cref="Firmware"/> class.
    /// </summary>
    public class FirmwareConfiguration : EntityBaseConfiguration<Firmware, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Firmware> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.FilterSchema);

            builder.HasOne(f => f.Device)
                .WithOne(d => d!.Firmware)
                .HasForeignKey<Firmware>(f => f.DeviceForeignKey);

            builder.Property(f => f.Source)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.Property(f => f.Upgrade)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.Property(f => f.Version)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();
#if DEBUG
            builder.HasData(DbSeed.Firmwares);
#endif
        }
    }
}
