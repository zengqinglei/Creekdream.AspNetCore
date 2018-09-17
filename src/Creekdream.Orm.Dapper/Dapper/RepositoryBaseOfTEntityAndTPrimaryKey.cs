using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Creekdream.Domain.Entities;
using Creekdream.Domain.Repositories;
using DapperExtensions;

namespace Creekdream.Orm.Dapper
{
    /// <inheritdoc />
    public class RepositoryBase<TEntity, TPrimaryKey> : AbsRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly IDatabaseProvider _databaseProvider;

        /// <inheritdoc />
        public RepositoryBase(IDatabaseProvider dbConnectionProvider)
        {
            _databaseProvider = dbConnectionProvider;
        }

        /// <summary>
        /// Gets Database Transaction
        /// </summary>
        public DbTransaction DbTransaction => _databaseProvider.DbTransaction;

        /// <summary>
        /// Gets Database Connection
        /// </summary>
        public IDatabase Database => _databaseProvider.GetDatabase();

        /// <inheritdoc />
        public override async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = Database.Get<TEntity>(
                id,
                transaction: DbTransaction);
            return await Task.FromResult(entity);
        }

        /// <inheritdoc />
        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetListAsync(predicate);
            return entities.Single();
        }

        /// <inheritdoc />
        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetListAsync(predicate);
            return entities.FirstOrDefault();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var predicateGroup = CreatePredicateGroup(predicate);
            var entities = Database.GetList<TEntity>(
                predicateGroup,
                null,
                transaction: DbTransaction);
            return await Task.FromResult(entities);
        }

        /// <inheritdoc />
        public override async Task<TEntity> InsertEntityAsync(TEntity entity)
        {
            var id = Database.Insert(
                entity,
                transaction: DbTransaction);
            return await GetAsync(id);
        }

        /// <inheritdoc />
        public override async Task<TEntity> UpdateEntityAsync(TEntity entity)
        {
            var isSuccess = Database.Update(
                entity,
                transaction: DbTransaction);
            if (!isSuccess)
            {
                throw new Exception($"Update entity failed. Entity type: {typeof(TEntity).FullName}, id: {entity.Id}");
            }
            return await GetAsync(entity.Id);
        }

        /// <inheritdoc />
        public override async Task DeleteEntityAsync(TEntity entity)
        {
            var isSuccess = Database.Delete(
                entity,
                transaction: DbTransaction);
            if (!isSuccess)
            {
                throw new Exception($"Delete entity failed. Entity type: {typeof(TEntity).FullName}, id: {entity.Id}");
            }
            await Task.FromResult(0);
        }

        /// <inheritdoc />
        public override async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var predicateGroup = CreatePredicateGroup(predicate);
            var count = Database.Count<TEntity>(
                predicateGroup,
                transaction: DbTransaction);
            return await Task.FromResult(count);
        }

        /// <summary>
        /// Create an Id equality expression
        /// </summary>
        protected Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        /// <summary>
        /// Create grouping conditions
        /// </summary>
        public IPredicate CreatePredicateGroup(Expression<Func<TEntity, bool>> predicate = null)
        {
            var groups = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            if (predicate != null)
            {
                var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                groups.Predicates.Add(pg);
            }
            return groups;
        }
    }
}

