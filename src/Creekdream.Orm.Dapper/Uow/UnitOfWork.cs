using Creekdream.Orm.Dapper;
using DapperExtensions;

namespace Creekdream.Uow
{
    /// <summary>
    /// Dapper implemented based on work unit
    /// </summary>
    public class UnitOfWork : UnitOfWorkBase
    {
        private UnitOfWorkOptions _uowOptions;
        private readonly IDatabaseProvider _dbConnectionProvider;
        private readonly IDatabase _database;

        /// <inheritdoc />
        public UnitOfWork(IDatabaseProvider dbConnectionProvider) : base()
        {
            _dbConnectionProvider = dbConnectionProvider;
            _database = _dbConnectionProvider.GetDatabase();
        }

        /// <inheritdoc />
        protected override void BeginUow(UnitOfWorkOptions uowOptions)
        {
            _uowOptions = uowOptions;
            if (_uowOptions.IsTransactional && _dbConnectionProvider.DbTransaction == null)
            {
                var isoLationLevel = ToSystemDataIsolationLevel(_uowOptions.IsolationLevel);
                _dbConnectionProvider.DbTransaction = _database.Connection.BeginTransaction(isoLationLevel);
            }
        }

        /// <inheritdoc />
        protected override void CompleteUow()
        {
            if (_uowOptions.IsTransactional == true)
            {
                _dbConnectionProvider.DbTransaction.Commit();
            }
        }

        /// <inheritdoc />
        protected override void DisposeUow()
        {
            _database.Dispose();
            if (_uowOptions.IsTransactional == true && _dbConnectionProvider.DbTransaction != null)
            {
                _dbConnectionProvider.DbTransaction.Dispose();
            }
        }
    }
}

