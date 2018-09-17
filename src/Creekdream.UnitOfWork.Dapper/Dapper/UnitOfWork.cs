using System;
using System.Data;
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
        private IDatabaseProvider _dbConnectionProvider;
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
            if (_uowOptions.IsTransactional == true && _dbConnectionProvider.DbTransaction == null)
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

        /// <summary>
        /// Transaction conversion
        /// </summary>
        private IsolationLevel ToSystemDataIsolationLevel(System.Transactions.IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case System.Transactions.IsolationLevel.Chaos:
                    return IsolationLevel.Chaos;
                case System.Transactions.IsolationLevel.ReadCommitted:
                    return IsolationLevel.ReadCommitted;
                case System.Transactions.IsolationLevel.ReadUncommitted:
                    return IsolationLevel.ReadUncommitted;
                case System.Transactions.IsolationLevel.RepeatableRead:
                    return IsolationLevel.RepeatableRead;
                case System.Transactions.IsolationLevel.Serializable:
                    return IsolationLevel.Serializable;
                case System.Transactions.IsolationLevel.Snapshot:
                    return IsolationLevel.Snapshot;
                case System.Transactions.IsolationLevel.Unspecified:
                    return IsolationLevel.Unspecified;
                default:
                    throw new Exception("Unknown isolation level: " + isolationLevel);
            }
        }
    }
}

