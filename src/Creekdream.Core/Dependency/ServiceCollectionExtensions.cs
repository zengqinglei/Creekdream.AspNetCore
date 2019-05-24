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
        public static IServiceCollection RegisterAssemblyByBasicInterface(this IServiceCollection services, Assembly assembly)
        {
            var registrar = new DefaultConventionalRegistrar();
            registrar.AddAssembly(services, assembly);
            return services;
        }

        /// <summary>
        /// Registration service based on event
        /// </summary>
        public static void OnRegistred(this IServiceCollection services, Action<IOnServiceRegistredContext> registrationAction)
        {
            ServiceRegistrationActionList actionList = null;
            var serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ServiceRegistrationActionList));
            if (serviceDescriptor != null)
            {
                actionList = (ServiceRegistrationActionList)serviceDescriptor.ImplementationInstance;
            }
            if (actionList == null)
            {
                actionList = new ServiceRegistrationActionList();
                services.AddSingleton(actionList);
            }
            actionList.Add(registrationAction);
        }
    }
}
