// <copyright file="IRepository.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Unifies the data access for client and server.
    /// Implementation of the repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <typeparam name="TKey">Type of the db key.</typeparam>
    public interface IRepository<TEntity, in TKey>
        where TEntity : class
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Creates a new db entry.
        /// </summary>
        /// <param name="entity">The entity to save in the db.</param>
        /// <returns>The success of the operation.</returns>
        Task<bool> CreateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entry from the db with the given id.
        /// </summary>
        /// <param name="id">The id from the entry to delete.</param>
        /// <returns>The success of the operation.</returns>
        Task<bool> DeleteAsync(TKey id);

        /// <summary>
        /// Gets all entries from the db table.
        /// NOTE: Use this to make selects on the table.
        /// </summary>
        /// <returns>Queryable object for db queries.</returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// Gets an entry from the db.
        /// </summary>
        /// <param name="id">The id from the db entry.</param>
        /// <returns>The complete entity or null, if the id not exists.</returns>
        Task<TEntity> GetAsync(TKey id);

        /// <summary>
        /// Updates an entry in the db.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The success of the operation.</returns>
        Task<bool> UpdateAsync(TEntity entity);
    }
}
