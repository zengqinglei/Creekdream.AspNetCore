using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;

namespace Creekdream.Dependency.Autofac
{
    /// <inheritdoc />
    public class AutofacIocResolver : IocResolverBase
    {
        private readonly IContainer _container;

        /// <inheritdoc />
        public AutofacIocResolver(IContainer container)
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
            var namedParameters = new List<Parameter>();
            foreach (var property in argumentsAsAnonymousType.GetType().GetProperties())
            {
                var namedParameter = new NamedParameter(property.Name, property.GetValue(argumentsAsAnonymousType));
                namedParameters.Add(namedParameter);
            }
            return _container.Resolve(serviceType, namedParameters);
        }

        /// <inheritdoc />
        public override bool IsRegistered(Type serviceType)
        {
            return _container.IsRegistered(serviceType);
        }

        /// <inheritdoc />
        public override void Release(object obj)
        {
            if (obj is IDisposable)
            {
                ((IDisposable)obj).Dispose();
            }
        }
    }
}

