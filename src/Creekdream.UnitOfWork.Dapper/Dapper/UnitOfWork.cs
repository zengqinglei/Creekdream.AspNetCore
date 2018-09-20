using System.Data.Common;
using Creekdream.Orm.Dapper;

namespace Creekdream.UnitOfWork.Dapper
{
    /// <summary>
    /// Dapper implemented based on work unit
    /// </summary>
    public class UnitOfWork : UnitOfWorkBase
    {
        private UnitOfWorkOptions _uowOptions;
        private readonly IDatabaseProvider _dbConnectionProvider;
        private readonly DbConnection _dbConnection;

        /// <inheritdoc />
        public UnitOfWork(IDatabaseProvider dbConnectionProvider) : base()
        {
            _dbConnectionProvider = dbConnectionProvider;
            _dbConnection = _dbConnectionProvider.GetDatabase().Connection;
        }

        /// <inheritdoc />
        protected override void BeginUow(UnitOfWorkOptions uowOptions)
        {
            _uowOptions = uowOptions;
            if (_uowOptions.IsTransactional && _dbConnectionProvider.DbTransaction == null)
            {
                var isoLationLevel = ToSystemDataIsolationLevel(_uowOptions.IsolationLevel);
                _dbConnectionProvider.DbTransaction = _dbConnection.BeginTransaction(isoLationLevel);
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
            _dbConnection.Dispose();
            if (_uowOptions.IsTransactional == true && _dbConnectionProvider.DbTransaction != null)
            {
                _dbConnectionProvider.DbTransaction.Dispose();
            }
        }
    }
}

