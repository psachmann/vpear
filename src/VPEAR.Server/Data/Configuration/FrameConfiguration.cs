// <copyright file="FrameConfiguration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using VPEAR.Core.Entities;
using VPEAR.Core.Extensions;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Data.Configuration
{
    /// <summary>
    /// The entity framework configuration for the <see cref="Frame"/> class.
    /// </summary>
    public class FrameConfiguration : AbstractEntityConfiguration<Frame, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Frame> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.FrameSchema);

            builder.HasOne(frame => frame.Device)
                .WithMany(device => device.Frames)
                .HasForeignKey(frame => frame.DeviceForeignKey);

            builder.Property(frame => frame.Readings)
                .IsRequired()
                .IsUnicode();

            builder.Property(frame => frame.Time)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();

            builder.HasOne(frame => frame.Filter)
                .WithMany(filter => filter.Frames)
                .HasForeignKey(frame => frame.FilterForeignKey);
#if DEBUG
            builder.HasData(DataSeed.Frames);
#endif
        }
    }
}
