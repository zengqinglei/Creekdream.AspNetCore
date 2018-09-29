using System;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Defining a resolver abstract class
    /// </summary>
    public abstract class IocResolverBase : IIocResolver
    {
        /// <inheritdoc />
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <inheritdoc />
        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return (T)Resolve(typeof(T), argumentsAsAnonymousType);
        }

        /// <inheritdoc />
        public object Resolve(Type serviceType)
        {
            return Resolve(serviceType, null);
        }

        /// <inheritdoc />
        public abstract object Resolve(Type serviceType, object argumentsAsAnonymousType = null);

        /// <inheritdoc />
        public bool IsRegistered<TService>()
        {
            return IsRegistered(typeof(TService));
        }

        /// <inheritdoc />
        public abstract bool IsRegistered(Type serviceType);

        /// <inheritdoc />
        public abstract void Release(object obj);

        /// <inheritdoc />
        public virtual void Dispose() { }
    }
}
