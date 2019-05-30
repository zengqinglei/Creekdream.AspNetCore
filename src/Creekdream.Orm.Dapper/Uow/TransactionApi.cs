using Creekdream.Uow;
using DapperExtensions;
using System.Data.Common;
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
        public DbTransaction DbTransaction { get; }
        /// <summary>
        /// Start dbContext
        /// </summary>
        public IDatabase StarterDatabase { get; }

        /// <inheritdoc />
        public TransactionApi(DbTransaction dbTransaction, IDatabase starterDatabase)
        {
            DbTransaction = dbTransaction;
            StarterDatabase = starterDatabase;
        }

        /// <inheritdoc />
        public void Commit()
        {
            DbTransaction.Commit();
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
            DbTransaction.Dispose();
        }

        /// <inheritdoc />
        public void Rollback()
        {
            DbTransaction.Rollback();
        }

        /// <inheritdoc />
        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            DbTransaction.Rollback();
            return Task.CompletedTask;
        }
    }
}
