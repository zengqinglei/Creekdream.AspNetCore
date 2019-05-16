using Creekdream.Dependency;
using Creekdream.Orm;
using DapperExtensions;
using DapperExtensions.Sql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;

namespace Creekdream.Uow
{
    /// <summary>
    /// Dapper implemented based on work unit
    /// </summary>
    public class UnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private UnitOfWorkOptions _uowOptions;
        private IDatabase _database;
        private DbTransaction _dbTransaction;

        /// <inheritdoc />
        public UnitOfWork(IServiceProvider serviceProvider) : base()
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        protected override void BeginUow(UnitOfWorkOptions uowOptions)
        {
            _uowOptions = uowOptions;
        }

        /// <summary>
        /// Get or create dbcontext
        /// </summary>
        public virtual IDatabase GetOrCreateDatabase()
        {
            if (_database == null)
            {
                var dapperOptions = _serviceProvider.GetRequiredService<DapperOptionsBuilder>();
                var config = new DapperExtensionsConfiguration(
                    dapperOptions.DefaultMapper,
                    dapperOptions.MapperAssemblies,
                    dapperOptions.SqlDialect);
                var sqlGenerator = new SqlGeneratorImpl(config);
                _database = new Database(dapperOptions.GetDbConnection(), sqlGenerator);
                if (_uowOptions.IsTransactional && _dbTransaction == null)
                {
                    var isoLationLevel = ToSystemDataIsolationLevel(_uowOptions.IsolationLevel);
                    _dbTransaction = _database.Connection.BeginTransaction(isoLationLevel);
                }
            }
            return _database;
        }

        /// <inheritdoc />
        protected override void CompleteUow()
        {
            if (_uowOptions.IsTransactional == true)
            {
                _dbTransaction.Commit();
            }
        }

        /// <inheritdoc />
        protected override void DisposeUow()
        {
            if (_uowOptions.IsTransactional == true && _dbTransaction != null)
            {
                _dbTransaction.Dispose();
            }
            _database.Dispose();
            _database = null;
        }
    }
}

