using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Creekdream.Dependency
{
    /// <summary>
    /// ServiceCollection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Get a single service
        /// </summary>
        private static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }

        /// <summary>
        /// Automatically scan the assembly service type and register
        /// </summary>
        public static IServiceCollection RegisterGeneric(
            this IServiceCollection services,
            Type serviceType,
            Type implementationType,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            var iocRegister = services.GetSingletonInstanceOrNull<IocRegisterBase>();
            iocRegister.RegisterGeneric(serviceType, implementationType, lifeStyle);
            return services;
        }

        /// <summary>
        /// Automatically scan the assembly service type and register
        /// </summary>
        public static IServiceCollection RegisterAssemblyByBasicInterface(this IServiceCollection services, Assembly assembly)
        {
            var iocRegister = services.GetSingletonInstanceOrNull<IocRegisterBase>();
            iocRegister.RegisterAssemblyByBasicInterface(assembly);
            return services;
        }

        /// <summary>
        /// Automatically scan the assembly service type and register
        /// </summary>
        public static IServiceCollection RegisterInterceptor<TInterceptor>(this IServiceCollection services, Func<TypeInfo, bool> filterCondition)
            where TInterceptor : InterceptorBase
        {
            var iocRegister = services.GetSingletonInstanceOrNull<IocRegisterBase>();
            iocRegister.RegisterInterceptor<TInterceptor>(filterCondition);
            return services;
        }

        /// <summary>
        /// Automatically scan the assembly service type and register
        /// </summary>
        public static IServiceProvider GetServiceProvider(this IServiceCollection services)
        {
            var iocRegister = services.GetSingletonInstanceOrNull<IocRegisterBase>();
            return iocRegister.GetServiceProvider(services);
        }
    }
}
