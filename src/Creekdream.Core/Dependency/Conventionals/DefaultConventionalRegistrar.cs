using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Creekdream.Dependency
{
    /// <summary>
    /// TODO: Make DefaultConventionalRegistrar extensible, so we can only define GetLifeTimeOrNull to contribute to the convention. This can be more performant!
    /// </summary>
    public class DefaultConventionalRegistrar : ConventionalRegistrarBase
    {
        /// <inheritdoc />
        public override void AddType(IServiceCollection services, Type type)
        {
            if (IsConventionalRegistrationDisabled(type))
            {
                return;
            }

            var dependencyAttribute = GetDependencyAttributeOrNull(type);
            var lifeTime = GetLifeTimeOrNull(type, dependencyAttribute);

            if (lifeTime == null)
            {
                return;
            }

            foreach (var serviceType in AutoRegistrationHelper.GetExposedServices(services, type))
            {
                var serviceDescriptor = ServiceDescriptor.Describe(serviceType, type, lifeTime.Value);

                if (dependencyAttribute?.ReplaceServices == true)
                {
                    services.Replace(serviceDescriptor);
                }
                else if (dependencyAttribute?.TryRegister == true)
                {
                    services.TryAdd(serviceDescriptor);
                }
                else
                {
                    services.Add(serviceDescriptor);
                }
            }
        }

        /// <inheritdoc />
        protected virtual bool IsConventionalRegistrationDisabled(Type type)
        {
            return type.IsDefined(typeof(DisableConventionalRegistrationAttribute), true);
        }

        /// <inheritdoc />
        protected virtual DependencyAttribute GetDependencyAttributeOrNull(Type type)
        {
            return type.GetCustomAttribute<DependencyAttribute>(true);
        }

        /// <inheritdoc />
        protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type, DependencyAttribute dependencyAttribute)
        {
            return dependencyAttribute?.Lifetime ?? GetServiceLifetimeFromClassHierarcy(type);
        }

        /// <inheritdoc />
        protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarcy(Type type)
        {
            if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            return null;
        }
    }
}
