using System.Threading;
using System.Threading.Tasks;

namespace Creekdream.Uow
{
    /// <summary>
    /// Supports saving changes
    /// </summary>
    public interface ISupportsSavingChanges
    {
        /// <summary>
        /// Save changes
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Async save changes
        /// </summary>
        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
