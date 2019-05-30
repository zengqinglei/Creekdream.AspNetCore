using System.Threading;
using System.Threading.Tasks;

namespace Creekdream.Uow
{
    /// <summary>
    /// Supports rollback
    /// </summary>
    public interface ISupportsRollback
    {
        /// <summary>
        /// Rollback
        /// </summary>
        void Rollback();

        /// <summary>
        /// Async rollback
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken);
    }
}
