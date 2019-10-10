using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Creekdream.Dependency.Autofac
{
    /// <summary>
    /// Service factory based on Autofac
    /// </summary>
    public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly Action<ContainerBuilder> _configurationAction;

        /// <inheritdoc />
        public AutofacServiceProviderFactory(Action<ContainerBuilder> configurationAction = null)
        {
            _configurationAction = configurationAction ?? (builder => { });
        }

        /// <inheritdoc />
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(new ServiceCollection());
            RegisterServices(builder, services);

            _configurationAction(builder);

            return builder;
        }

        /// <inheritdoc />
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            if (containerBuilder == null)
            {
                throw new ArgumentNullException(nameof(containerBuilder));
            }

            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        private void RegisterServices(ContainerBuilder builder, IServiceCollection services)
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

            foreach (var service in services)
            {
                if (service.ImplementationType != null)
                {
                    // Test if the an open generic type is being registered
                    var serviceTypeInfo = service.ServiceType.GetTypeInfo();
                    if (serviceTypeInfo.IsGenericTypeDefinition)
                    {
                        builder
                            .RegisterGeneric(service.ImplementationType)
                            .As(service.ServiceType)
                            .ConfigureLifecycle(service.Lifetime)
                            .ConfigureConventions(actionList);
                    }
                    else
                    {
                        builder
                            .RegisterType(service.ImplementationType)
                            .As(service.ServiceType)
                            .ConfigureLifecycle(service.Lifetime)
                            .ConfigureConventions(actionList);
                    }
                }
                else if (service.ImplementationFactory != null)
                {
                    var registration = RegistrationBuilder.ForDelegate(service.ServiceType, (context, parameters) =>
                    {
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return service.ImplementationFactory(serviceProvider);
                    })
                    .ConfigureLifecycle(service.Lifetime)
                    .CreateRegistration();
                    //TODO: ConfigureAbpConventions ?

                    builder.RegisterComponent(registration);
                }
                else
                {
                    builder
                        .RegisterInstance(service.ImplementationInstance)
                        .As(service.ServiceType)
                        .ConfigureLifecycle(service.Lifetime);
                }
            }
        }
    }
}
