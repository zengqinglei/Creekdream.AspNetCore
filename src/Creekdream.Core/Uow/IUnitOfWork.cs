using System;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkCompleteHandle
    {
        /// <summary>
        /// This event is raised when this UOW is successfully completed.
        /// </summary>
        event EventHandler Completed;

        /// <summary>
        /// This event is raised when this UOW is failed.
        /// </summary>
        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// This event is raised when this UOW is disposed.
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// The only unit of work Id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Outer unit of work
        /// </summary>
        IUnitOfWork Outer { get; set; }

        /// <summary>
        /// Open unit of work
        /// </summary>
        void Begin(UnitOfWorkOptions options);

        /// <summary>
        /// Is disposed
        /// </summary>
        bool IsDisposed { get; }
    }
}

