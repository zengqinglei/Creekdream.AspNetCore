using Creekdream.Orm.EntityFrameworkCore;
using Creekdream.Uow;
using System.Threading;
using System.Threading.Tasks;

namespace Creekdream.Orm.Uow
{
    /// <inheritdoc />
    public class DatabaseApi<TDbContext> : IDatabaseApi, ISupportsSavingChanges
        where TDbContext : DbContextBase
    {
        /// <inheritdoc />
        public TDbContext DbContext { get; }

        /// <inheritdoc />
        public DatabaseApi(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <inheritdoc />
        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
