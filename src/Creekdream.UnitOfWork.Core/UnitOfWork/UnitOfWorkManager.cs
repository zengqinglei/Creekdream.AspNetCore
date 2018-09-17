using System.Transactions;
using Creekdream.Dependency;

namespace Creekdream.UnitOfWork
{
    /// <inheritdoc />
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly UnitOfWorkOptions _defaultUowOptions;
        private readonly IIocResolver _iocResolver;
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        /// <inheritdoc />
        public UnitOfWorkManager(
            UnitOfWorkOptions uowOptions,
            IIocResolver iocManager,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _defaultUowOptions = uowOptions;
            _iocResolver = iocManager;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        /// <inheritdoc />
        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options = null)
        {
            if (options == null)
            {
                options = _defaultUowOptions;
            }

            var outerUow = _currentUnitOfWorkProvider.Current;

            if (options.Scope == TransactionScopeOption.Required && outerUow != null)
            {
                return new InnerUnitOfWorkCompleteHandle();
            }

            var uow = _iocResolver.Resolve<IUnitOfWork>();

            uow.Begin(options);

            _currentUnitOfWorkProvider.Current = uow;

            return uow;
        }
    }
}

