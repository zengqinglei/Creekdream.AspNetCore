using System;
using Creekdream.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Uow
{
    /// <inheritdoc />
    public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
    {
        /// <inheritdoc />
        public IUnitOfWork Current => GetCurrentUnitOfWork();

        private readonly UnitOfWorkOptions _defaultUowOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAmbientUnitOfWork _ambientUnitOfWork;

        /// <inheritdoc />
        public UnitOfWorkManager(
            UnitOfWorkOptions uowOptions,
            IServiceProvider serviceProvider,
            IAmbientUnitOfWork ambientUnitOfWork)
        {
            _defaultUowOptions = uowOptions;
            _serviceProvider = serviceProvider;
            _ambientUnitOfWork = ambientUnitOfWork;
        }

        /// <inheritdoc />
        public IUnitOfWork Begin(UnitOfWorkOptions options = null, bool requiresNew = true)
        {
            if (options == null && requiresNew)
            {
                options = _defaultUowOptions.Clone();
                options.IsTransactional = true;
            }
            var currentUow = GetCurrentUnitOfWork();
            if (currentUow != null && !requiresNew)
            {
                return new ChildUnitOfWork(currentUow);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Initialize(options);

            return unitOfWork;
        }

        private IUnitOfWork GetCurrentUnitOfWork()
        {
            var uow = _ambientUnitOfWork.Get();

            //Skip reserved unit of work
            while (uow != null && (uow.IsDisposed || uow.IsCompleted))
            {
                uow = uow.Outer;
            }

            return uow;
        }

        private IUnitOfWork CreateNewUnitOfWork()
        {
            var scope = _serviceProvider.CreateScope();
            try
            {
                var outerUow = _ambientUnitOfWork.Get();

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                unitOfWork.SetOuter(outerUow);

                _ambientUnitOfWork.Set(unitOfWork);

                unitOfWork.Disposed += (sender, args) =>
                {
                    _ambientUnitOfWork.Set(outerUow);
                    scope.Dispose();
                };

                return unitOfWork;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}

