// <copyright file="Repository.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<IRepository<TEntity, TKey>> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TDbContext, TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="context">The connection to the db.</param>
        /// <param name="logger">The repository logger.</param>
        public Repository(TDbContext context, ILogger<IRepository<TEntity, TKey>> logger)
        {
            this.context = context;
            this.logger = logger;
#if DEBUG
            // TODO: is populating still needed?
            // populates the seed data into the in memory db
            this.context.Database.EnsureCreated();
#endif
        }

        /// <inheritdoc/>
        public async Task<bool> CreateAsync(TEntity entity)
        {
            try
            {
                await this.context.Set<TEntity>().AddAsync(entity);
                await this.context.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                this.logger.LogError("Message: \"{@Error}\"", exception.Message);
                this.logger.LogDebug("Exception: \"{@Debug}\"", exception);

                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                this.context.Set<TEntity>().Remove(entity);
                await this.context.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                this.logger.LogError("Message: \"{@Error}\"", exception.Message);
                this.logger.LogDebug("Exception: \"{@Debug}\"", exception);

                return false;
            }
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Get()
        {
            return this.context.Set<TEntity>().AsNoTracking();
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetAsync(TKey id)
        {
            try
            {
                return await this.context.Set<TEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => id.Equals(e.Id));
            }
            catch (Exception exception)
            {
                this.logger.LogError("Message: \"{@Error}\"", exception.Message);
                this.logger.LogDebug("Exception: \"{@Debug}\"", exception);

                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                this.context.Set<TEntity>().Update(entity);
                await this.context.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                this.logger.LogError("Message: \"{@Error}\"", exception.Message);
                this.logger.LogDebug("Exception: \"{@Debug}\"", exception);

                return false;
            }
        }
    }
}
