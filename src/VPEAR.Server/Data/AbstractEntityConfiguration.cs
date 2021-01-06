// <copyright file="AbstractEntityConfiguration.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using VPEAR.Core.Abstractions;

namespace VPEAR.Server.Data
{
    /// <summary>
    /// Base class for all db entity configuration classes.
    /// Configure all properties from <see cref="AbstractEntity{TKey}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the db entity to configure.</typeparam>
    /// <typeparam name="TKey">Type of the db key for db entity to configure.</typeparam>
    public abstract class AbstractEntityConfiguration<TEntity, TKey> : AbstractEntity<TKey>, IEntityTypeConfiguration<TEntity>
        where TEntity : AbstractEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Configures the entity of type TEntity.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.CreatedAt)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(entity => entity.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(entity => entity.ModifiedAt)
                .IsConcurrencyToken()
                .IsRequired()
                .IsRowVersion()
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
