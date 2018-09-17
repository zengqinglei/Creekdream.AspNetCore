using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;

namespace Creekdream.Dependency.Autofac
{
    /// <inheritdoc />
    public class AutofacIocResolver : IIocResolver
    {
        private readonly ILifetimeScope _lifetimeScope;

        /// <inheritdoc />
        public AutofacIocResolver(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        /// <inheritdoc />
        public T Resolve<T>()
        {
            return _lifetimeScope.Resolve<T>();
        }

        /// <inheritdoc />
        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            var namedParameters = new List<Parameter>();
            foreach (var property in argumentsAsAnonymousType.GetType().GetProperties())
            {
                var namedParameter = new NamedParameter(property.Name, property.GetValue(argumentsAsAnonymousType));
                namedParameters.Add(namedParameter);
            }
            return _lifetimeScope.Resolve<T>(namedParameters);
        }

        /// <inheritdoc />
        public object Resolve(Type serviceType)
        {
            return _lifetimeScope.Resolve(serviceType);
        }

        /// <inheritdoc />
        public object Resolve(Type serviceType, object argumentsAsAnonymousType)
        {
            var namedParameters = new List<Parameter>();
            foreach (var property in argumentsAsAnonymousType.GetType().GetProperties())
            {
                var namedParameter = new NamedParameter(property.Name, property.GetValue(argumentsAsAnonymousType));
                namedParameters.Add(namedParameter);
            }
            return _lifetimeScope.Resolve(serviceType, namedParameters);
        }

        /// <inheritdoc />
        public bool IsRegistered(Type serviceType)
        {
            return _lifetimeScope.IsRegistered(serviceType);
        }

        /// <inheritdoc />
        public bool IsRegistered<TService>()
        {
            return _lifetimeScope.IsRegistered<TService>();
        }

        /// <inheritdoc />
        public void Release(object obj)
        {
            if (obj is IDisposable)
            {
                ((IDisposable)obj).Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }
}

