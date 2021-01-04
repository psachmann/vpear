// <copyright file="DeviceConfiguration.cs" company="Patrick Sachmann">
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
    /// The entity framework configuration for the <see cref="Device"/> class.
    /// </summary>
    public class DeviceConfiguration : EntityBaseConfiguration<Device, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Device> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.DeviceSchema);

            builder.Property(d => d.Address)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.Property(d => d.Class)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.HasOne(d => d.Filters)
                .WithOne();

            builder.HasOne(d => d.Firmware)
                .WithOne();

            builder.HasMany(d => d.Frames)
                .WithOne();

            builder.Property(d => d.Name)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.HasMany(d => d.Sensors)
                .WithOne();

            builder.HasOne(d => d.Wifi)
                .WithOne();
#if DEBUG
            builder.HasData(DbSeed.Devices);
#endif
        }
    }
}
