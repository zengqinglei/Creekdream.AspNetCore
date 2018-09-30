using System.Data.Common;
using Creekdream.Dependency;
using Creekdream.Threading;
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

        private readonly IAsyncLocalObjectProvider _asyncLocalObjectProvider;
        private readonly IIocResolver _iocResolver;

        /// <inheritdoc />
        public DatabaseProvider(IAsyncLocalObjectProvider asyncLocalObjectProvider, IIocResolver iocResolver)
        {
            _asyncLocalObjectProvider = asyncLocalObjectProvider;
            _iocResolver = iocResolver;
        }

        /// <inheritdoc />
        public IDatabase GetDatabase()
        {
            var localDatabase = _asyncLocalObjectProvider.GetCurrent<LocalDatabaseWapper>();
            if (localDatabase == null || localDatabase.Database == null)
            {
                var dapperOptions = _iocResolver.Resolve<DapperOptionsBuilder>();
                var config = new DapperExtensionsConfiguration(
                    dapperOptions.DefaultMapper,
                    dapperOptions.MapperAssemblies,
                    dapperOptions.SqlDialect);
                var sqlGenerator = new SqlGeneratorImpl(config);
                localDatabase = new LocalDatabaseWapper()
                {
                    Database = new Database(dapperOptions.GetDbConnection(), sqlGenerator)
                };
                _asyncLocalObjectProvider.SetCurrent(localDatabase);
            }
            return localDatabase.Database;
        }

        /// <inheritdoc />
        public DbTransaction DbTransaction
        {
            get => _asyncLocalObjectProvider.GetCurrent<LocalDatabaseWapper>()?.DbTransaction;
            set => _asyncLocalObjectProvider.GetCurrent<LocalDatabaseWapper>().DbTransaction = value;
        }
    }
}

