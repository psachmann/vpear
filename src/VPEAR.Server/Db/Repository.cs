// <copyright file="Repository.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;

namespace VPEAR.Server.Db
{
    /// <summary>
    /// Implements IRepository{TEntity, TKey} interface.
    /// </summary>
    /// <typeparam name="TDbContext">Type of the db context.</typeparam>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <typeparam name="TKey">Type of the db id.</typeparam>
    public class Repository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : EntityBase<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        private readonly TDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TDbContext, TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="context">The connection to the db.</param>
        public Repository(TDbContext context)
        {
            this.context = context;
#if DEBUG
            // TODO: is populating still needed?
            // populates the seed data into the in memory db
            this.context.Database.EnsureCreated();
#endif
        }

        /// <inheritdoc/>
        public async Task<bool> CreateAsync(TEntity entity)
        {
            await this.context.Set<TEntity>().AddAsync(entity);
            await this.context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await this.GetAsync(id);
            this.context.Set<TEntity>().Remove(entity);
            await this.context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Get()
        {
            return this.context.Set<TEntity>().AsNoTracking();
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetAsync(TKey id)
        {
            return await this.context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => id.Equals(e.Id));
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            this.context.Set<TEntity>().Update(entity);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
