using Creekdream.Domain.Entities;
using Creekdream.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Creekdream.Domain.Repositories
{
    /// <inheritdoc />
    public abstract class AbsRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <inheritdoc />
        public IServiceProvider ServiceProvider { get; }

        /// <inheritdoc />
        public AbsRepositoryBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        #region Select

        /// <inheritdoc />
        public abstract Task<TEntity> GetAsync(TPrimaryKey id);
        /// <inheritdoc />
        public abstract Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        /// <inheritdoc />
        public abstract Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        /// <inheritdoc />
        public abstract Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null);

        #endregion

        #region Insert

        /// <inheritdoc />
        public Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity is IHasCreationTime && (entity as IHasCreationTime).CreationTime == default)
            {
                (entity as IHasCreationTime).CreationTime = DateTime.Now;
            }
            return InsertEntityAsync(entity);
        }

        /// <inheritdoc />
        public abstract Task<TEntity> InsertEntityAsync(TEntity entity);

        #endregion

        #region Update

        /// <inheritdoc />
        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity is IHasModificationTime)
            {
                (entity as IHasModificationTime).LastModificationTime = DateTime.Now;
            }
            return UpdateEntityAsync(entity);
        }

        /// <inheritdoc />
        public abstract Task<TEntity> UpdateEntityAsync(TEntity entity);

        #endregion

        #region Delete

        /// <inheritdoc />
        public async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = await GetAsync(id);
            await DeleteAsync(entity);
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            foreach (var entity in await GetListAsync(predicate))
            {
                await DeleteAsync(entity);
            }
        }

        /// <inheritdoc />
        public Task DeleteAsync(TEntity entity)
        {
            if (entity is ISoftDelete)
            {
                (entity as ISoftDelete).IsDeleted = true;
                if (entity is IHasDeletionTime)
                {
                    (entity as IHasDeletionTime).DeletionTime = DateTime.Now;
                }
                return UpdateEntityAsync(entity);
            }
            else
            {
                return DeleteEntityAsync(entity);
            }
        }

        /// <inheritdoc />
        public abstract Task DeleteEntityAsync(TEntity entity);

        #endregion

        #region Aggregates

        /// <inheritdoc />
        public abstract Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null);

        #endregion
    }
}