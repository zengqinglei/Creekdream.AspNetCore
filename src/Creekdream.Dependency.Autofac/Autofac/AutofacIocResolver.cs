using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;

namespace Creekdream.Dependency.Autofac
{
    /// <inheritdoc />
    public class AutofacIocResolver : IocResolverBase
    {
        private bool isDispose = false;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IContainer _container;

        /// <inheritdoc />
        public AutofacIocResolver(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _container = _lifetimeScope.Resolve<IContainer>();
        }

        private IComponentContext GetComponentContext()
        {
            if (isDispose)
            {
                return _container.BeginLifetimeScope();
            }
            else
            {
                return _lifetimeScope;
            }
        }

        /// <inheritdoc />
        public override object Resolve(Type serviceType, object argumentsAsAnonymousType = null)
        {
            if (argumentsAsAnonymousType == null)
            {
                return GetComponentContext().Resolve(serviceType);
            }
            var namedParameters = new List<Parameter>();
            foreach (var property in argumentsAsAnonymousType.GetType().GetProperties())
            {
                var namedParameter = new NamedParameter(property.Name, property.GetValue(argumentsAsAnonymousType));
                namedParameters.Add(namedParameter);
            }
            return GetComponentContext().Resolve(serviceType, namedParameters);
        }

        /// <inheritdoc />
        public override bool IsRegistered(Type serviceType)
        {
            return GetComponentContext().IsRegistered(serviceType);
        }

        /// <inheritdoc />
        public override void Release(object obj)
        {
            if (obj is IDisposable)
            {
                ((IDisposable)obj).Dispose();
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            isDispose = true;
            base.Dispose();
        }
    }
}

