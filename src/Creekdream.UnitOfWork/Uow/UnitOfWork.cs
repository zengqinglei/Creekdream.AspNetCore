﻿using Creekdream.Dependency;
using Creekdream.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit Of Work
    /// </summary>
    public class UnitOfWork : IUnitOfWork, ITransientDependency
    {
        /// <inheritdoc/>
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc/>
        public IUnitOfWorkOptions Options { get; private set; }

        /// <inheritdoc/>
        public IUnitOfWork Outer { get; private set; }

        /// <inheritdoc/>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public bool IsCompleted { get; private set; }

        /// <inheritdoc/>
        protected List<Func<Task>> CompletedHandlers { get; } = new List<Func<Task>>();

        /// <inheritdoc/>
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <inheritdoc/>
        public event EventHandler<UnitOfWorkEventArgs> Disposed;

        /// <inheritdoc/>
        public IServiceProvider ServiceProvider { get; }

        private IDatabaseApi _databaseApi;
        private ITransactionApi _transactionApi;

        private Exception _exception;
        private bool _isCompleting;
        private bool _isRolledback;

        /// <inheritdoc/>
        public UnitOfWork(IServiceProvider serviceProvider, UnitOfWorkOptions options)
        {
            ServiceProvider = serviceProvider;
            Options = options.Clone();
        }

        /// <inheritdoc/>
        public virtual void Initialize(UnitOfWorkOptions options)
        {
            if (options != null)
            {
                Options = options;
            }
        }

        /// <inheritdoc/>
        public virtual void SetOuter(IUnitOfWork outer)
        {
            Outer = outer;
        }

        /// <inheritdoc/>
        public virtual void Complete()
        {
            if (_isRolledback)
            {
                return;
            }

            PreventMultipleComplete();

            try
            {
                _isCompleting = true;
                var databaseApi = (_databaseApi as ISupportsSavingChanges);
                if (databaseApi != null)
                {
                    databaseApi.SaveChanges();
                }
                CommitTransactions();
                IsCompleted = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            if (_isRolledback)
            {
                return;
            }

            PreventMultipleComplete();

            try
            {
                _isCompleting = true;
                var databaseApi = (_databaseApi as ISupportsSavingChanges);
                if (databaseApi != null)
                {
                    await databaseApi.SaveChangesAsync(cancellationToken);
                }
                await CommitTransactionsAsync();
                IsCompleted = true;
                await OnCompletedAsync();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual void Rollback()
        {
            if (_isRolledback)
            {
                return;
            }

            _isRolledback = true;

            RollbackAll();
        }

        /// <inheritdoc/>
        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_isRolledback)
            {
                return;
            }

            _isRolledback = true;

            await RollbackAllAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IDatabaseApi GetOrAddDatabaseApi(Func<IDatabaseApi> factory)
        {
            if (_databaseApi == null)
            {
                _databaseApi = factory();
            }
            return _databaseApi;
        }

        /// <inheritdoc/>
        public ITransactionApi FindTransactionApi()
        {
            return _transactionApi;
        }

        /// <inheritdoc/>
        public void AddTransactionApi(ITransactionApi api)
        {
            _transactionApi = api;
        }

        /// <inheritdoc/>
        public void OnCompleted(Func<Task> handler)
        {
            CompletedHandlers.Add(handler);
        }

        /// <inheritdoc/>
        public void OnFailed(Func<Task> handler)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected virtual void OnCompleted()
        {
            foreach (var handler in CompletedHandlers)
            {
                AsyncHelper.RunSync(handler);
            }
        }

        /// <inheritdoc/>
        protected virtual async Task OnCompletedAsync()
        {
            foreach (var handler in CompletedHandlers)
            {
                await handler.Invoke();
            }
        }

        /// <inheritdoc/>
        protected virtual void OnFailed()
        {
            Failed?.Invoke(this, new UnitOfWorkFailedEventArgs(this, _exception, _isRolledback));
        }

        /// <inheritdoc/>
        protected virtual void OnDisposed()
        {
            Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            DisposeDatabases();
            DisposeTransactions();

            if (!IsCompleted || _exception != null)
            {
                OnFailed();
            }

            OnDisposed();
        }

        private void DisposeDatabases()
        {
            try
            {
                if (_databaseApi != null)
                {
                    _databaseApi.Dispose();
                }
            }
            catch
            {
            }
        }

        private void DisposeTransactions()
        {
            try
            {
                if (_transactionApi != null)
                {
                    _transactionApi.Dispose();
                }
            }
            catch
            {
            }
        }

        private void PreventMultipleComplete()
        {
            if (IsCompleted || _isCompleting)
            {
                throw new Exception("Complete is called before!");
            }
        }

        /// <inheritdoc/>
        protected virtual void RollbackAll()
        {
            try
            {
                var databaseApi = (_databaseApi as ISupportsRollback);
                if (databaseApi != null)
                {
                    databaseApi.Rollback();
                }
                var transactionApi = (_transactionApi as ISupportsRollback);
                if (transactionApi != null)
                {
                    transactionApi.Rollback();
                }
            }
            catch
            {
            }
        }

        /// <inheritdoc/>
        protected virtual async Task RollbackAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var databaseApi = (_databaseApi as ISupportsRollback);
                if (databaseApi != null)
                {
                    await databaseApi.RollbackAsync(cancellationToken);
                }
                var transactionApi = (_transactionApi as ISupportsRollback);
                if (transactionApi != null)
                {
                    await transactionApi.RollbackAsync(cancellationToken);
                }
            }
            catch { }
        }

        /// <inheritdoc/>
        protected virtual void CommitTransactions()
        {
            if (_transactionApi != null)
            {
                _transactionApi.Commit();
            }
        }

        /// <inheritdoc/>
        protected virtual async Task CommitTransactionsAsync()
        {
            if (_transactionApi != null)
            {
                await _transactionApi.CommitAsync();
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }
    }
}