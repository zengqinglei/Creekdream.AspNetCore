using Creekdream.DynamicProxy;
using Creekdream.System.Collections;
using System;

namespace Creekdream.Dependency
{
    /// <inheritdoc />
    public class OnServiceRegistredContext : IOnServiceRegistredContext
    {
        /// <inheritdoc />
        public virtual ITypeList<InterceptorBase> Interceptors { get; }

        /// <inheritdoc />
        public virtual Type ServiceType { get; }

        /// <inheritdoc />
        public virtual Type ImplementationType { get; }

        /// <inheritdoc />
        public OnServiceRegistredContext(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;

            Interceptors = new TypeList<InterceptorBase>();
        }
    }
}
