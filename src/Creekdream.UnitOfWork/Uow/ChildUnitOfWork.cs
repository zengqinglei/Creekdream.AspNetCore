using System;
using System.Threading;
using System.Threading.Tasks;

namespace Creekdream.Uow
{
    /// <summary>
    /// Internal unit of work handler
    /// </summary>
    internal class ChildUnitOfWork : IUnitOfWork
    {
        public Guid Id => _parent.Id;

        public IUnitOfWorkOptions Options => _parent.Options;

        public IUnitOfWork Outer => _parent.Outer;

        public bool IsDisposed => _parent.IsDisposed;

        public bool IsCompleted => _parent.IsCompleted;

        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
        public event EventHandler<UnitOfWorkEventArgs> Disposed;

        public IServiceProvider ServiceProvider => _parent.ServiceProvider;

        private readonly IUnitOfWork _parent;

        public ChildUnitOfWork(IUnitOfWork parent)
        {
            _parent = parent;

            _parent.Failed += (sender, args) =>
            {
                Failed?.Invoke(sender, args);
            };
            _parent.Disposed += (sender, args) =>
            {
                Disposed?.Invoke(sender, args);
            };
        }

        public void SetOuter(IUnitOfWork outer)
        {
            _parent.SetOuter(outer);
        }

        public void Initialize(UnitOfWorkOptions options)
        {
            _parent.Initialize(options);
        }

        public void Complete()
        {

        }

        public Task CompleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public void Rollback()
        {
            _parent.Rollback();
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _parent.RollbackAsync(cancellationToken);
        }

        public void OnCompleted(Func<Task> handler)
        {
            _parent.OnCompleted(handler);
        }

        public IDatabaseApi GetOrAddDatabaseApi(Func<IDatabaseApi> factory)
        {
            return _parent.GetOrAddDatabaseApi(factory);
        }

        public ITransactionApi FindTransactionApi()
        {
            return _parent.FindTransactionApi();
        }

        public void AddTransactionApi(ITransactionApi api)
        {
            _parent.AddTransactionApi(api);
        }

        public void Dispose()
        {

        }

        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }
    }
}

