using Creekdream.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Creekdream.Domain.Repositories
{
    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// </summary>
    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select

        /// <summary>
        /// Finds an entity with the given primary key values, and throws an exception If this entity does not exist.
        /// </summary>
        Task<TEntity> GetAsync(TPrimaryKey id);

        /// <summary>
        /// Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.
        /// </summary>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition or a default value if no such element is found.
        /// </summary>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get entity collections based on criteria
        /// </summary>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null);

        #endregion

        #region Insert

        /// <summary>
        /// Add an entity
        /// </summary>
        Task<TEntity> InsertAsync(TEntity entity);

        #endregion

        #region Update
        /// <summary>
        /// Update an entity
        /// </summary>
        Task<TEntity> UpdateAsync(TEntity entity);

        #endregion

        #region Delete

        /// <summary>
        /// Delete an entity by id
        /// </summary>
        Task DeleteAsync(TPrimaryKey id);

        /// <summary>
        /// Delete an entity
        /// </summary>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete entities under the conditions
        /// </summary>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate = null);

        #endregion

        #region Aggregates

        /// <summary>
        /// Get total number of elements in a sequence.
        /// </summary>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null);

        #endregion
    }
}