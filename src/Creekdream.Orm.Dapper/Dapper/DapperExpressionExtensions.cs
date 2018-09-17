using System;
using System.Linq.Expressions;
using Creekdream.Domain.Entities;
using DapperExtensions;

namespace Creekdream.Orm.Dapper
{
    /// <summary>
    /// Dapper specific extension methods for <see cref="Expression" />
    /// </summary>
    internal static class DapperExpressionExtensions
    {
        /// <summary>
        /// Linq expression convert to dapper predicate
        /// </summary>
        public static IPredicate ToPredicateGroup<TEntity, TPrimaryKey>(this Expression<Func<TEntity, bool>> expression)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var dev = new DapperExpressionVisitor<TEntity, TPrimaryKey>();
            IPredicate pg = dev.Process(expression);

            return pg;
        }
    }
}

