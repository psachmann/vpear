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
    public class DeviceConfiguration : AbstractEntityConfiguration<Device, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Device> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.DeviceSchema);

            builder.Property(device => device.Address)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.Property(device => device.Class)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.Property(device => device.DisplayName)
                .HasMaxLength(Limits.MaxStringLength)
                .IsUnicode();

            builder.HasOne(device => device.Filter)
                .WithOne();

            builder.HasOne(device => device.Firmware)
                .WithOne();

            builder.HasMany(device => device.Frames)
                .WithOne();

            builder.Property(device => device.Name)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.HasMany(device => device.Sensors)
                .WithOne();

            builder.HasOne(device => device.Wifi)
                .WithOne();
#if DEBUG
            builder.HasData(SeedData.Devices);
#endif
        }
    }
}
