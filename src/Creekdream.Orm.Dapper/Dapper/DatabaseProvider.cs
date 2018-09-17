using System.Data.Common;
using Creekdream.Dependency;
using DapperExtensions;
using DapperExtensions.Sql;

namespace Creekdream.Orm.Dapper
{
    /// <inheritdoc />
    public class DatabaseProvider : IDatabaseProvider
    {
        private IDatabase _database;
        private readonly IIocResolver _iocResolver;

        /// <inheritdoc />
        public DatabaseProvider(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        /// <inheritdoc />
        public IDatabase GetDatabase()
        {
            if (_database == null)
            {
                var dapperOptions = _iocResolver.Resolve<DapperOptionsBuilder>();
                var config = new DapperExtensionsConfiguration(
                    dapperOptions.DefaultMapper,
                    dapperOptions.MapperAssemblies,
                    dapperOptions.SqlDialect);
                var sqlGenerator = new SqlGeneratorImpl(config);
                _database = new Database(dapperOptions.GetDbConnection(), sqlGenerator);
            }
            return _database;
        }

        /// <inheritdoc />
        public DbTransaction DbTransaction { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_database != null)
            {
                _database = null;
            }
        }
    }
}

