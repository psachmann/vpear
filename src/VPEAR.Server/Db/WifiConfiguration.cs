// <copyright file="WifiConfiguration.cs" company="Patrick Sachmann">
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
    /// The entity framework configuration for the <see cref="Wifi"/> class.
    /// </summary>
    public class WifiConfiguration : EntityBaseConfiguration<Wifi, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Wifi> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.WifiSchema);

            builder.HasOne(w => w.Device)
                .WithOne(d => d!.Wifi)
                .HasForeignKey<Wifi>(w => w.DeviceForeignKey);

            builder.Property(w => w.Mode)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.Property(w => w.Neighbors)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(new[] { ';' }));

            builder.Property(w => w.Ssid)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();
        }
    }
}
