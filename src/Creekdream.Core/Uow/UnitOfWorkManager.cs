using System;
using System.Transactions;
using Creekdream.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Uow
{
    /// <inheritdoc />
    public class UnitOfWorkManager : IUnitOfWorkManager, ITransientDependency
    {
        private readonly UnitOfWorkOptions _defaultUowOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        /// <inheritdoc />
        public UnitOfWorkManager(
            UnitOfWorkOptions uowOptions,
            IServiceProvider serviceProvider,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _defaultUowOptions = uowOptions;
            _serviceProvider = serviceProvider;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        /// <inheritdoc />
        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options = null)
        {
            if (options == null)
            {
                options = _defaultUowOptions;
            }

            var outerUow = _currentUnitOfWorkProvider.Get();

            if (outerUow != null)
            {
                return new InnerUnitOfWorkCompleteHandle();
            }

            var scope = _serviceProvider.CreateScope();
            try
            {
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                uow.Completed += (sender, args) =>
                {
                    _currentUnitOfWorkProvider.Set(null);
                };

                uow.Failed += (sender, args) =>
                {
                    _currentUnitOfWorkProvider.Set(null);
                };

                uow.Disposed += (sender, args) =>
                {
                    scope.Dispose();
                };

                uow.Begin(options);

                _currentUnitOfWorkProvider.Set(uow);

                return uow;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}

