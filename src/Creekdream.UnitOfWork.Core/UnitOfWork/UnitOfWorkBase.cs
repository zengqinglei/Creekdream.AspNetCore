using System;

namespace Creekdream.UnitOfWork
{
    /// <summary>
    /// Base for all Unit Of Work classes.
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public IUnitOfWork Outer { get; set; }

        /// <inheritdoc />
        public bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public UnitOfWorkBase()
        {
            Id = Guid.NewGuid().ToString("N");
        }


        private bool _isBeginCalledBefore;
        /// <inheritdoc />
        public void Begin(UnitOfWorkOptions options)
        {
            PreventMultipleBegin();

            BeginUow(options);
        }

        private void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new Exception("This unit of work has started before. Can not call Start method more than once.");
            }

            _isBeginCalledBefore = true;
        }

        /// <summary>
        /// Can be implemented by derived classes to start UOW.
        /// </summary>
        protected virtual void BeginUow(UnitOfWorkOptions curUowOptions)
        {

        }


        private bool _isCompleteCalledBefore;
        /// <inheritdoc />
        public void Complete()
        {
            PreventMultipleComplete();
            CompleteUow();
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new Exception("Complete is called before!");
            }

            _isCompleteCalledBefore = true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isBeginCalledBefore || IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            DisposeUow();
        }

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        /// Should be implemented by derived classes to dispose UOW.
        /// </summary>
        protected abstract void DisposeUow();
    }
}