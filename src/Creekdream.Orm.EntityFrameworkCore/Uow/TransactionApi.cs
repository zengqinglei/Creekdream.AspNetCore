using Creekdream.Orm.EntityFrameworkCore;
using Creekdream.Uow;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Creekdream.Orm.Uow
{
    /// <inheritdoc />
    public class TransactionApi : ITransactionApi, ISupportsRollback
    {
        /// <summary>
        /// DbContext transaction
        /// </summary>
        public IDbContextTransaction DbContextTransaction { get; }
        /// <summary>
        /// Start dbContext
        /// </summary>
        public DbContextBase StarterDbContext { get; }
        /// <summary>
        /// Attended dbContext
        /// </summary>
        public List<DbContextBase> AttendedDbContexts { get; }

        /// <inheritdoc />
        public TransactionApi(IDbContextTransaction dbContextTransaction, DbContextBase starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;
            AttendedDbContexts = new List<DbContextBase>();
        }

        /// <inheritdoc />
        public void Commit()
        {
            DbContextTransaction.Commit();

            foreach (var dbContext in AttendedDbContexts)
            {
                if (dbContext.HasRelationalTransactionManager())
                {
                    continue; //Relational databases use the shared transaction
                }

                dbContext.Database.CommitTransaction();
            }
        }

        /// <inheritdoc />
        public Task CommitAsync()
        {
            Commit();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            DbContextTransaction.Dispose();
        }

        /// <inheritdoc />
        public void Rollback()
        {
            DbContextTransaction.Rollback();
        }

        /// <inheritdoc />
        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            DbContextTransaction.Rollback();
            return Task.CompletedTask;
        }
    }
}
