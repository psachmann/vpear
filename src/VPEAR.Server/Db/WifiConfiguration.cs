// <copyright file="WifiConfiguration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using VPEAR.Core.Extensions;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Db
{
    /// <summary>
    /// The entity framework configuration for the <see cref="Core.Models.Wifi"/> class.
    /// </summary>
    public class WifiConfiguration : EntityBaseConfiguration<Core.Models.Wifi, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Core.Models.Wifi> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.WifiSchema);

            builder.HasOne(w => w.Device)
                .WithOne(d => d.Wifi)
                .HasForeignKey<Core.Models.Wifi>(w => w.DeviceForeignKey);

            builder.Property(w => w.Mode)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.Property(w => w.Neighbors)
                .HasConversion(
                    value => value.ToJsonString(),
                    value => value.FromJsonString<IList<string>>());

            builder.Property(w => w.Ssid)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();
#if DEBUG
            builder.HasData(DbSeed.Wifis);
#endif
        }
    }
}
