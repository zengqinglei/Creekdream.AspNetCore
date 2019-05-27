using System;
using System.Threading.Tasks;
using Creekdream.Dependency;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work completion processing
    /// </summary>
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// Submit a unit of work
        /// </summary>
        void Complete();

        /// <summary>
        /// Async submit a unit of work
        /// </summary>
        Task CompleteAsync();
    }
}

