using Creekdream.Dependency;
using System.Threading;

namespace Creekdream.Uow
{
    /// <inheritdoc />
    public class CurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider, ISingletonDependency
    {
        private readonly AsyncLocal<LocalUowWrapper> _currentUow = new AsyncLocal<LocalUowWrapper>();

        /// <inheritdoc />
        public IUnitOfWork Get()
        {
            var uow = _currentUow.Value?.UnitOfWork;
            if (uow == null)
            {
                return null;
            }

            if (uow.IsDisposed)
            {
                _currentUow.Value = null;
                return null;
            }

            return uow;
        }

        /// <inheritdoc />
        public void Set(IUnitOfWork value)
        {
            lock (_currentUow)
            {
                if (value == null)
                {
                    if (_currentUow.Value == null)
                    {
                        return;
                    }

                    if (_currentUow.Value.UnitOfWork?.Outer == null)
                    {
                        _currentUow.Value.UnitOfWork = null;
                        _currentUow.Value = null;
                        return;
                    }

                    _currentUow.Value.UnitOfWork = _currentUow.Value.UnitOfWork.Outer;
                }
                else
                {
                    if (_currentUow.Value?.UnitOfWork == null)
                    {
                        if (_currentUow.Value != null)
                        {
                            _currentUow.Value.UnitOfWork = value;
                        }

                        _currentUow.Value = new LocalUowWrapper(value);
                        return;
                    }

                    value.Outer = _currentUow.Value.UnitOfWork;
                    _currentUow.Value.UnitOfWork = value;
                }
            }
        }

        private class LocalUowWrapper
        {
            public IUnitOfWork UnitOfWork { get; set; }

            public LocalUowWrapper(IUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }
        }
    }
}
