using System;
using Creekdream.Dependency;

namespace Creekdream.UnitOfWork
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

