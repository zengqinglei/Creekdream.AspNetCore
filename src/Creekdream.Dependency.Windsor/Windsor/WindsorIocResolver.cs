using Castle.Windsor;
using System;

namespace Creekdream.Dependency.Windsor
{
    /// <inheritdoc />
    public class WindsorIocResolver : IocResolverBase
    {
        private readonly IWindsorContainer _container;

        /// <inheritdoc />
        public WindsorIocResolver(IWindsorContainer container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public override object Resolve(Type serviceType, object argumentsAsAnonymousType = null)
        {
            if (argumentsAsAnonymousType == null)
            {
                return _container.Resolve(serviceType);
            }
            return _container.Resolve(serviceType, argumentsAsAnonymousType);
        }

        /// <inheritdoc />
        public override bool IsRegistered(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType);
        }

        /// <inheritdoc />
        public override void Release(object obj)
        {
            _container.Release(obj);
        }
    }
}

