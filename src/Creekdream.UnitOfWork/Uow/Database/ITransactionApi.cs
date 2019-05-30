using System;
using System.Threading.Tasks;

namespace Creekdream.Uow
{
    /// <summary>
    /// Transaction api
    /// </summary>
    public interface ITransactionApi : IDisposable
    {
        /// <summary>
        /// Commit
        /// </summary>
        void Commit();

        /// <summary>
        /// Async commit
        /// </summary>
        Task CommitAsync();
    }
}
