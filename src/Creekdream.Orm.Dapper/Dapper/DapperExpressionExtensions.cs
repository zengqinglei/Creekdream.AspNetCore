using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Creekdream.Domain.Entities;
using DapperExtensions;

namespace Creekdream.Orm.Dapper
{
    /// <summary>
    /// Dapper specific extension methods for <see cref="Expression" />
    /// </summary>
    public static class DapperExpressionExtensions
    {
        /// <summary>
        /// Linq expression convert to dapper predicate
        /// </summary>
        public static IPredicate ToPredicate<TEntity, TPrimaryKey>(this Expression<Func<TEntity, bool>> expression)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var dev = new DapperExpressionVisitor<TEntity, TPrimaryKey>();
            return dev.Process(expression);
        }

        /// <summary>
        /// Create grouping conditions
        /// </summary>
        public static IPredicateGroup ToPredicateGroup<TEntity, TPrimaryKey>(
            this Expression<Func<TEntity, bool>> predicate,
            GroupOperator groupOperator = GroupOperator.And)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var groups = new PredicateGroup
            {
                Operator = groupOperator,
                Predicates = new List<IPredicate>()
            };
            if (predicate != null)
            {
                var pg = predicate.ToPredicate<TEntity, TPrimaryKey>();
                groups.Predicates.Add(pg);
            }
            return groups;
        }
    }
}

