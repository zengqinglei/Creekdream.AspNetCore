using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Creekdream.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Creekdream.Dependency.Windsor
{
    /// <summary>
    /// Service factory based on Autofac
    /// </summary>
    public class WindsorServiceProviderFactory : IServiceProviderFactory<IWindsorContainer>
    {
        private readonly IWindsorContainer _container;
        private IServiceCollection _services;

        /// <inheritdoc />
        public WindsorServiceProviderFactory(WindsorContainer container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public IWindsorContainer CreateBuilder(IServiceCollection services)
        {
            _services = services;
            RegisterInterceptors(services);

            return _container;
        }

        /// <inheritdoc />
        public IServiceProvider CreateServiceProvider(IWindsorContainer container)
        {
            return WindsorRegistrationHelper.CreateServiceProvider(container, _services);
        }

        private void RegisterInterceptors(IServiceCollection services)
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

            _container.Kernel.ComponentRegistered += (key, handler) =>
            {
                var serviceType = handler.ComponentModel.Services.FirstOrDefault()?.GetTypeInfo();
                if (serviceType == null)
                {
                    return;
                }

                var implementationType = handler.ComponentModel.Implementation.GetTypeInfo();
                if (implementationType == null)
                {
                    return;
                }


                var serviceRegistredArgs = new OnServiceRegistredContext(serviceType, implementationType);

                foreach (var registrationAction in actionList)
                {
                    registrationAction.Invoke(serviceRegistredArgs);
                }

                if (serviceRegistredArgs.Interceptors.Any())
                {
                    foreach (var interceptor in serviceRegistredArgs.Interceptors)
                    {
                        handler.ComponentModel.Interceptors.Add(
                            new InterceptorReference(interceptor)
                        );
                    }
                }
            };
        }
    }
}
