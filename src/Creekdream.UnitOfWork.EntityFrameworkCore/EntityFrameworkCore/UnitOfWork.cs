using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Creekdream.Orm.EntityFrameworkCore;

namespace Creekdream.UnitOfWork.EntityFrameworkCore
{
    /// <summary>
    /// EfCore implemented based on work unit
    /// </summary>
    public class UnitOfWork : UnitOfWorkBase
    {
        private UnitOfWorkOptions _uowOptions;
        private TransactionScope _transactionScope;
        private readonly DbContext _dbContext;

        /// <inheritdoc />
        public UnitOfWork(IDbContextProvider dbContextProvider) : base()
        {
            _dbContext = dbContextProvider.GetDbContext();
        }

        /// <inheritdoc />
        protected override void BeginUow(UnitOfWorkOptions uowOptions)
        {
            _uowOptions = uowOptions;
            if (_uowOptions.IsTransactional == true && _transactionScope == null)
            {
                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = _uowOptions.IsolationLevel,
                };

                if (_uowOptions.Timeout.HasValue)
                {
                    transactionOptions.Timeout = _uowOptions.Timeout.Value;
                }

                _transactionScope = new TransactionScope(
                    _uowOptions.Scope,
                    transactionOptions,
                    _uowOptions.AsyncFlowOption
                );
            }
        }

        /// <inheritdoc />
        protected override void CompleteUow()
        {
            if (_uowOptions.IsTransactional == true)
            {
                _transactionScope.Complete();
            }
        }

        /// <inheritdoc />
        protected override void DisposeUow()
        {
            _dbContext.Dispose();
            if (_uowOptions.IsTransactional == true && _transactionScope != null)
            {
                _transactionScope.Dispose();
            }
        }
    }
}

