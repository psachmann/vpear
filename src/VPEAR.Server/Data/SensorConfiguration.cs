// <copyright file="SensorConfiguration.cs" company="Patrick Sachmann">
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
    /// The entity framework configuration for the <see cref="Sensor"/> class.
    /// </summary>
    public class SensorConfiguration : AbstractEntityConfiguration<Sensor, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Sensor> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.SensorSchema);

            builder.HasOne(sensor => sensor.Device)
                .WithMany(device => device.Sensors)
                .HasForeignKey(sensor => sensor.DeviceForeignKey);

            builder.Property(sensor => sensor.Name)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.Property(sensor => sensor.Units)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();
#if DEBUG
            builder.HasData(DbSeed.Sensors);
#endif
        }
    }
}
