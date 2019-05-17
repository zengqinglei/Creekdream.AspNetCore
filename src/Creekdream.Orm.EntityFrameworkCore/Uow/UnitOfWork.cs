using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Creekdream.Dependency;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Creekdream.Orm.EntityFrameworkCore;

namespace Creekdream.Uow
{
    /// <summary>
    /// EfCore implemented based on work unit
    /// </summary>
    public class UnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private UnitOfWorkOptions _uowOptions;
        private DbContextBase _dbContext;
        private IDbContextTransaction _dbContextTransaction;

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
        public virtual DbContextBase GetOrCreateDbContext()
        {
            if (_dbContext == null)
            {
                _dbContext = _serviceProvider.GetRequiredService<DbContextBase>();
                if (_uowOptions.IsTransactional)
                {
                    if (_dbContextTransaction == null)
                    {
                        var isoLationLevel = ToSystemDataIsolationLevel(_uowOptions.IsolationLevel);
                        _dbContextTransaction = _dbContext.Database.BeginTransaction(isoLationLevel);
                    }
                    else
                    {
                        _dbContext.Database.UseTransaction(_dbContextTransaction.GetDbTransaction());
                    }
                }

                bool isRelational = _dbContext.Database.GetInfrastructure().GetService<IRelationalConnection>() != null;
                if (_uowOptions.Timeout.HasValue &&
                   isRelational &&
                   !_dbContext.Database.GetCommandTimeout().HasValue)
                {
                    _dbContext.Database.SetCommandTimeout((int)_uowOptions.Timeout.Value.TotalSeconds);
                }
            }
            return _dbContext;
        }

        /// <inheritdoc />
        protected override void CompleteUow()
        {
            if (_uowOptions.IsTransactional == true)
            {
                _dbContextTransaction.Commit();
            }
        }

        /// <inheritdoc />
        protected override void DisposeUow()
        {
            if (_uowOptions.IsTransactional && _dbContextTransaction != null)
            {
                _dbContextTransaction.Dispose();
            }
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }
    }
}

