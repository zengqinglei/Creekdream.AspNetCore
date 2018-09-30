using System.Transactions;
using Creekdream.Dependency;
using Creekdream.Threading;

namespace Creekdream.UnitOfWork
{
    /// <inheritdoc />
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private class LocalUowWrapper
        {
            public IUnitOfWork UnitOfWork { get; set; }

            public LocalUowWrapper(IUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }
        }

        private readonly UnitOfWorkOptions _defaultUowOptions;
        private readonly IIocResolver _iocResolver;
        private readonly IAsyncLocalObjectProvider _asyncLocalObjectProvider;

        /// <inheritdoc />
        public UnitOfWorkManager(
            UnitOfWorkOptions uowOptions,
            IIocResolver iocManager,
            IAsyncLocalObjectProvider asyncLocalObjectProvider)
        {
            _defaultUowOptions = uowOptions;
            _iocResolver = iocManager;
            _asyncLocalObjectProvider = asyncLocalObjectProvider;
        }

        /// <inheritdoc />
        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options = null)
        {
            if (options == null)
            {
                options = _defaultUowOptions;
            }

            var outerUow = GetCurrentUow();

            if (options.Scope == TransactionScopeOption.Required && outerUow != null)
            {
                return new InnerUnitOfWorkCompleteHandle();
            }

            var uow = _iocResolver.Resolve<IUnitOfWork>();

            uow.Begin(options);

            SetCurrentUow(uow);

            return uow;
        }

        private IUnitOfWork GetCurrentUow()
        {
            var uow = _asyncLocalObjectProvider.GetCurrent<LocalUowWrapper>()?.UnitOfWork;
            if (uow == null)
            {
                return null;
            }

            if (uow.IsDisposed)
            {
                _asyncLocalObjectProvider.SetCurrent<LocalUowWrapper>(null);
                return null;
            }

            return uow;
        }

        private void SetCurrentUow(IUnitOfWork value)
        {
            lock (_asyncLocalObjectProvider)
            {
                var currentUowWrapper = _asyncLocalObjectProvider.GetCurrent<LocalUowWrapper>();
                if (value == null)
                {
                    if (currentUowWrapper == null)
                    {
                        return;
                    }

                    if (currentUowWrapper.UnitOfWork?.Outer == null)
                    {
                        _asyncLocalObjectProvider.SetCurrent<LocalUowWrapper>(null);
                        return;
                    }

                    currentUowWrapper.UnitOfWork = currentUowWrapper.UnitOfWork.Outer;
                }
                else
                {
                    if (currentUowWrapper?.UnitOfWork == null)
                    {
                        _asyncLocalObjectProvider.SetCurrent(new LocalUowWrapper(value));
                        return;
                    }

                    value.Outer = currentUowWrapper.UnitOfWork;
                    currentUowWrapper.UnitOfWork = value;
                }
            }
        }
    }
}

