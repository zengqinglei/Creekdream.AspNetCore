using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Creekdream.Domain.Entities;
using Creekdream.Domain.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <summary>
    /// Repository extension method
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Manual navigation query
        /// </summary>
        public static IQueryable<TEntity> GetQueryIncluding<TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository,
            params Expression<Func<TEntity, object>>[] propertySelectors)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var table = repository.GetDbContext().Set<TEntity>();
            var query = table.AsQueryable();

            if (propertySelectors != null && propertySelectors.Count() > 0)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        /// <summary>
        /// Sql statement query result
        /// </summary>
        public static async Task<IEnumerable<TOutput>> QueryAsync<TEntity, TPrimaryKey, TOutput>(
            this IRepository<TEntity, TPrimaryKey> repository,
            string sql,
            DbParameter[] parameters = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var command = await repository.GetDbCommand(sql, parameters: parameters);
            return command.Parse<TOutput>();
        }

        /// <summary>
        /// Execute sql statement and return the number of affected rows
        /// </summary>
        public static async Task<int> ExecuteNonQueryAsync<TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository,
            string sql,
            DbParameter[] parameters = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var command = await repository.GetDbCommand(sql, parameters: parameters);
            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Execute the sql statement and query the first column value of the first row of the result
        /// </summary>
        public static async Task<object> ExecuteScalarAsync<TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository,
            string sql,
            DbParameter[] parameters = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var command = await repository.GetDbCommand(sql, parameters: parameters);
            return await command.ExecuteScalarAsync();
        }

        /// <summary>
        /// Get an dbcontext
        /// </summary>
        private static DbContextBase GetDbContext<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var dbContextProvider = repository.ServiceProvider.GetRequiredService<IDbContextProvider>();
            return dbContextProvider.GetDbContext();
        }

        /// <summary>
        /// Get an database command
        /// </summary>
        private static async Task<DbCommand> GetDbCommand<TEntity, TPrimaryKey>(
            this IRepository<TEntity, TPrimaryKey> repository,
            string sql,
            DbParameter[] parameters = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var database = repository.GetDbContext().Database;
            var dbConnection = database.GetDbConnection();
            var command = dbConnection.CreateCommand();
            command.Transaction = database.CurrentTransaction?.GetDbTransaction();
            if (command.Connection.State != ConnectionState.Open)
            {
                await command.Connection.OpenAsync();
            }

            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        /// <summary>
        /// Map data from datareader to object
        /// </summary>
        private static IEnumerable<T> Parse<T>(this DbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var props = typeof(T).GetRuntimeProperties();
                    var colMapping = new Dictionary<string, int>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        if (props.Any(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase) ||
                           columnName.Equals(p.GetCustomAttribute<ColumnAttribute>()?.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            colMapping.Add(columnName.ToLower(), i);
                        }
                    }
                    do
                    {
                        yield return reader.Map<T>(props, colMapping);
                    } while (reader.Read());
                }
            }
        }

        private static TEntity Map<TEntity>(
            this DbDataReader reader,
            IEnumerable<PropertyInfo> props,
            Dictionary<string, int> colMapping)
        {
            var item = Activator.CreateInstance<TEntity>();
            foreach (var prop in props)
            {
                var columnName = prop.GetCustomAttribute<ColumnAttribute>()?.Name?.ToLower() ?? prop.Name.ToLower();
                if (!colMapping.ContainsKey(columnName)) continue;
                var propValue = reader.GetValue(colMapping[columnName]);
                if (prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(item, int.Parse(propValue.ToString()) == 1);
                }
                else if (prop.PropertyType.GenericTypeArguments != null && prop.PropertyType.GenericTypeArguments.Any()
                    && prop.PropertyType.GenericTypeArguments[0].IsEnum)
                {
                    prop.SetValue(item, propValue == DBNull.Value ? null : Enum.Parse(prop.PropertyType.GenericTypeArguments[0], propValue.ToString()));
                }
                else if (prop.PropertyType.IsValueType && IsNullableType(prop.PropertyType))
                {
                    prop.SetValue(item, propValue == DBNull.Value || string.IsNullOrWhiteSpace(propValue.ToString()) ?
                        null : Convert.ChangeType(propValue, Nullable.GetUnderlyingType(prop.PropertyType)));
                }
                else
                {
                    prop.SetValue(item, propValue == DBNull.Value ? null : propValue);
                }
            }
            return item;
        }

        private static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }
    }
}

