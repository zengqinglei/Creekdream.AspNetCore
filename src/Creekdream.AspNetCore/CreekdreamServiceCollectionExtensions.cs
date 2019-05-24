using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Creekdream.AspNetCore
{
    /// <summary>
    /// Creekdream application service injection
    /// </summary>
    public static class CreekdreamServiceCollectionExtensions
    {
        /// <summary>
        /// Add application framework service
        /// </summary>
        public static IServiceProvider AddCreekdream(this IServiceCollection services, Action<ServicesBuilderOptions> options = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var builder = new ServicesBuilderOptions(services);
            options?.Invoke(builder);
            builder.Initialize();

            return services.BuildServiceProviderFromFactory();
        }

        /// <summary>
        /// Build service provider
        /// </summary>
        public static IServiceProvider BuildServiceProviderFromFactory(this IServiceCollection services)
        {
            foreach (var service in services)
            {
                var factoryInterface = service.ImplementationInstance?.GetType()
                    .GetTypeInfo()
                    .GetInterfaces()
                    .FirstOrDefault(i => i.GetTypeInfo().IsGenericType &&
                                         i.GetTypeInfo() != typeof(IServiceProviderFactory<IServiceCollection>) &&
                                         i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>));

                if (factoryInterface == null)
                {
                    continue;
                }

                var containerBuilderType = factoryInterface.GenericTypeArguments[0];
                return (IServiceProvider)typeof(CreekdreamServiceCollectionExtensions)
                    .GetTypeInfo()
                    .GetMethods()
                    .Single(m => m.Name == nameof(BuildServiceProviderFromFactory) && m.IsGenericMethod)
                    .MakeGenericMethod(containerBuilderType)
                    .Invoke(null, new object[] { services, null });
            }

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Build service provider
        /// </summary>
        public static IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>(this IServiceCollection services, Action<TContainerBuilder> builderAction = null)
        {
            var serviceProviderFactory = (IServiceProviderFactory<TContainerBuilder>)services
                .FirstOrDefault(d => d.ServiceType == typeof(IServiceProviderFactory<TContainerBuilder>))?
                .ImplementationInstance;
            if (serviceProviderFactory == null)
            {
                throw new Exception($"Could not find {typeof(IServiceProviderFactory<TContainerBuilder>).FullName} in {services}.");
            }

            var builder = serviceProviderFactory.CreateBuilder(services);
            builderAction?.Invoke(builder);
            return serviceProviderFactory.CreateServiceProvider(builder);
        }
    }
}

