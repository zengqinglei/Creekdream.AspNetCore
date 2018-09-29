using Microsoft.EntityFrameworkCore;
using Creekdream.Orm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Creekdream.UnitOfWork.EntityFrameworkCore
{
    /// <summary>
    /// EfCore implemented based on work unit
    /// </summary>
    public class UnitOfWork : UnitOfWorkBase
    {
        private UnitOfWorkOptions _uowOptions;
        private readonly IDbContextProvider _dbContextProvider;
        private readonly DbContext _dbContext;

        /// <inheritdoc />
        public UnitOfWork(IDbContextProvider dbContextProvider) : base()
        {
            _dbContextProvider = dbContextProvider;
            _dbContext = dbContextProvider.GetDbContext();
        }

        /// <inheritdoc />
        protected override void BeginUow(UnitOfWorkOptions uowOptions)
        {
            _uowOptions = uowOptions;
            if (_uowOptions.IsTransactional)
            {
                if (_dbContextProvider.DbContextTransaction == null)
                {
                    var isoLationLevel = ToSystemDataIsolationLevel(_uowOptions.IsolationLevel);
                    _dbContextProvider.DbContextTransaction = _dbContext.Database.BeginTransaction(isoLationLevel);
                }
                else
                {
                    var dbTransaction = _dbContextProvider.DbContextTransaction.GetDbTransaction();
                    _dbContext.Database.UseTransaction(dbTransaction);
                }
            }
        }

        /// <inheritdoc />
        protected override void CompleteUow()
        {
            if (_uowOptions.IsTransactional == true)
            {
                _dbContextProvider.DbContextTransaction.Commit();
            }
        }

        /// <inheritdoc />
        protected override void DisposeUow()
        {
            if (_uowOptions.IsTransactional && _dbContextProvider.DbContextTransaction != null)
            {
                _dbContextProvider.DbContextTransaction.Dispose();
            }
            _dbContext.Dispose();
        }
    }
}

