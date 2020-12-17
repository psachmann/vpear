// <copyright file="FrameConfiguration.cs" company="Patrick Sachmann">
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
    /// The entity framework configuration for the <see cref="Frame"/> class.
    /// </summary>
    public class FrameConfiguration : EntityBaseConfiguration<Frame, Guid>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Frame> builder)
        {
            base.Configure(builder);

            builder.ToTable(Schemas.FrameSchema);

            builder.HasOne(f => f.Device)
                .WithMany(d => d!.Frames)
                .HasForeignKey(f => f.DeviceForeignKey);

            builder.Property(f => f.Time)
                .HasMaxLength(Limits.MaxStringLength)
                .IsRequired()
                .IsUnicode();
#if DEBUG
            builder.HasData(DbSeed.Frames);
#endif
        }
    }
}
