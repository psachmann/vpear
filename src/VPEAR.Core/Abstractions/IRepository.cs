// <copyright file="IRepository.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Unifies the data access for client and server.
    /// Implementation of the repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <typeparam name="TKey">Type of the database key.</typeparam>
    public interface IRepository<TEntity, in TKey>
        where TEntity : class
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Creates a new database entry.
        /// </summary>
        /// <param name="entity">The entity to save in the database.</param>
        /// <returns>The success of the operation.</returns>
        Task<bool> CreateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entry from the database.
        /// </summary>
        /// <param name="entity">The entry to delete.</param>
        /// <returns>The success of the operation.</returns>
        Task<bool> DeleteAsync(TEntity entity);

        /// <summary>
        /// Gets all entries from the database table.
        /// NOTE: Use this to make selects on the table.
        /// </summary>
        /// <returns><see cref="Queryable"/> object for database queries.</returns>
        IEnumerable<TEntity> Get();

        /// <summary>
        /// Gets an entry from the database.
        /// </summary>
        /// <param name="id">The id from the database entry.</param>
        /// <returns>The complete entity or null, if id not exists.</returns>
        Task<TEntity?> GetAsync(TKey id);

        /// <summary>
        /// Updates an entry in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The success of the operation.</returns>
        Task<bool> UpdateAsync(TEntity entity);
    }
}
