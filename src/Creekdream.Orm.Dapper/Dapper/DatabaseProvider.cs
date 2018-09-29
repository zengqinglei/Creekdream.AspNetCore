using System.Data.Common;
using System.Threading;
using Creekdream.Dependency;
using DapperExtensions;
using DapperExtensions.Sql;

namespace Creekdream.Orm.Dapper
{
    /// <inheritdoc />
    public class DatabaseProvider : IDatabaseProvider
    {
        private class LocalDatabaseWapper
        {
            public IDatabase Database { get; set; }

            public DbTransaction DbTransaction { get; set; }
        }

        private static readonly AsyncLocal<LocalDatabaseWapper> AsyncLocalDatabase = new AsyncLocal<LocalDatabaseWapper>();

        private readonly IIocResolver _iocResolver;

        /// <inheritdoc />
        public DatabaseProvider(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        /// <inheritdoc />
        public IDatabase GetDatabase()
        {
            if (AsyncLocalDatabase.Value == null)
            {
                var dapperOptions = _iocResolver.Resolve<DapperOptionsBuilder>();
                var config = new DapperExtensionsConfiguration(
                    dapperOptions.DefaultMapper,
                    dapperOptions.MapperAssemblies,
                    dapperOptions.SqlDialect);
                var sqlGenerator = new SqlGeneratorImpl(config);
                AsyncLocalDatabase.Value = new LocalDatabaseWapper()
                {
                    Database = new Database(dapperOptions.GetDbConnection(), sqlGenerator)
                };
            }
            return AsyncLocalDatabase.Value.Database;
        }

        /// <inheritdoc />
        public DbTransaction DbTransaction
        {
            get
            {
                return AsyncLocalDatabase.Value.DbTransaction;
            }
            set
            {
                AsyncLocalDatabase.Value.DbTransaction = value;
            }
        }
    }
}

