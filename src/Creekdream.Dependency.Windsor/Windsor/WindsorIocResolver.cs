using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using System;

namespace Creekdream.Dependency.Windsor
{
    /// <inheritdoc />
    public class WindsorIocResolver : IIocResolver
    {
        private readonly IWindsorContainer _container;
        private readonly IDisposable _disposable;

        /// <inheritdoc />
        public WindsorIocResolver(IWindsorContainer container, bool isBenginScope = true)
        {
            _container = container;
            if (isBenginScope)
            {
                _disposable = _container.BeginScope();
            }
        }

        /// <inheritdoc />
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <inheritdoc />
        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return _container.Resolve<T>(argumentsAsAnonymousType);
        }

        /// <inheritdoc />
        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        /// <inheritdoc />
        public object Resolve(Type serviceType, object argumentsAsAnonymousType)
        {
            return _container.Resolve(serviceType, argumentsAsAnonymousType);
        }

        /// <inheritdoc />
        public bool IsRegistered(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType);
        }

        /// <inheritdoc />
        public bool IsRegistered<TService>()
        {
            return _container.Kernel.HasComponent(typeof(TService));
        }

        /// <inheritdoc />
        public void Release(object obj)
        {
            _container.Release(obj);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}

