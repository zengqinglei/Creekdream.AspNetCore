using System;
using Creekdream.Dependency;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work completion processing
    /// </summary>
    public interface IUnitOfWorkCompleteHandle : ITransientDependency, IDisposable
    {
        /// <summary>
        /// Submit a unit of work
        /// </summary>
        void Complete();
    }
}

