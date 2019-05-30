using System;
using System.Threading;
using System.Threading.Tasks;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork : IDatabaseApiContainer, ITransactionApiContainer, IDisposable
    {
        /// <summary>
        /// The only unit of work Id
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// This event is raised when this UOW is failed.
        /// TODO: Switch to OnFailed (sync) and OnDisposed (sync) methods to be compatible with OnCompleted
        /// </summary>
        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// This event is raised when this UOW is failed.
        /// </summary>
        event EventHandler<UnitOfWorkEventArgs> Disposed;

        /// <summary>
        /// Service provider
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Current unit of work options
        /// </summary>
        IUnitOfWorkOptions Options { get; }

        /// <summary>
        /// Outer unit of work
        /// </summary>
        IUnitOfWork Outer { get; }

        /// <summary>
        /// Is disposed
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Is completed
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Set outer unit of work
        /// </summary>
        void SetOuter(IUnitOfWork outer);

        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize(UnitOfWorkOptions options);

        /// <summary>
        /// Submit
        /// </summary>
        void Complete();

        /// <summary>
        /// Async submit
        /// </summary>
        Task CompleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rollback
        /// </summary>
        void Rollback();

        /// <summary>
        /// Async rollback
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Completed event
        /// </summary>
        /// <param name="handler"></param>
        void OnCompleted(Func<Task> handler);
    }
}

