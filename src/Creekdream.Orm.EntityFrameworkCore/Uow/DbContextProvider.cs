using Creekdream.Orm.EntityFrameworkCore;
using Creekdream.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream.Orm.Uow
{
    /// <inheritdoc />
    public class DbContextProvider : IDbContextProvider
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <inheritdoc />
        public DbContextProvider(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <inheritdoc />
        public DbContextBase GetDbContext()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new Exception("A DbContext can only be created inside a unit of work!");
            }

            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                () => new DatabaseApi<DbContextBase>(
                    CreateDbContext(unitOfWork)
                ));

            return ((DatabaseApi<DbContextBase>)databaseApi).DbContext;
        }

        private DbContextBase CreateDbContext(IUnitOfWork unitOfWork)
        {
            var creationContext = new DbContextCreationContext();
            using (DbContextCreationContext.Use(creationContext))
            {
                var dbContext = unitOfWork.Options.IsTransactional
                ? CreateDbContextWithTransaction(unitOfWork)
                : unitOfWork.ServiceProvider.GetRequiredService<DbContextBase>();

                if (unitOfWork.Options.Timeout.HasValue &&
                    dbContext.Database.IsRelational() &&
                    !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    dbContext.Database.SetCommandTimeout((int)unitOfWork.Options.Timeout.Value.TotalSeconds);
                }

                return dbContext;
            }
        }

        private DbContextBase CreateDbContextWithTransaction(IUnitOfWork unitOfWork)
        {
            var activeTransaction = unitOfWork.FindTransactionApi() as TransactionApi;

            if (activeTransaction == null)
            {
                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<DbContextBase>();

                var dbtransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? dbContext.Database.BeginTransaction(unitOfWork.Options.IsolationLevel.Value)
                    : dbContext.Database.BeginTransaction();

                unitOfWork.AddTransactionApi(
                    new TransactionApi(
                        dbtransaction,
                        dbContext
                    )
                );

                return dbContext;
            }
            else
            {
                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<DbContextBase>();

                if (dbContext.HasRelationalTransactionManager())
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction(); //TODO: Why not using the new created transaction?
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);

                return dbContext;
            }
        }
    }
}
