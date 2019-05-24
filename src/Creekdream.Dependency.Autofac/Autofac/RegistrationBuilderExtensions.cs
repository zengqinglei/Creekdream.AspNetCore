using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Creekdream.Dependency.Autofac
{
    /// <summary>
    /// Autofac RegistrationBuilder extensions
    /// </summary>
    public static class RegistrationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> ConfigureConventions<TLimit, TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
                ServiceRegistrationActionList registrationActionList)
            where TActivatorData : ReflectionActivatorData
        {
            var serviceType = registrationBuilder.RegistrationData.Services.OfType<IServiceWithType>().FirstOrDefault()?.ServiceType;
            if (serviceType == null)
            {
                return registrationBuilder;
            }

            var implementationType = registrationBuilder.ActivatorData.ImplementationType;
            if (implementationType == null)
            {
                return registrationBuilder;
            }

            registrationBuilder = registrationBuilder.PropertiesAutowired();
            registrationBuilder = registrationBuilder.InvokeRegistrationActions(registrationActionList, serviceType, implementationType);

            return registrationBuilder;
        }

        private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InvokeRegistrationActions<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder, ServiceRegistrationActionList registrationActionList, Type serviceType, Type implementationType)
            where TActivatorData : ReflectionActivatorData
        {
            var serviceRegistredArgs = new OnServiceRegistredContext(serviceType, implementationType);

            foreach (var registrationAction in registrationActionList)
            {
                registrationAction.Invoke(serviceRegistredArgs);
            }

            if (serviceRegistredArgs.Interceptors.Any())
            {
                registrationBuilder = registrationBuilder.AddInterceptors(
                    serviceType,
                    serviceRegistredArgs.Interceptors
                );
            }

            return registrationBuilder;
        }

        private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> AddInterceptors<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
            Type serviceType,
            IEnumerable<Type> interceptors)
            where TActivatorData : ReflectionActivatorData
        {
            if (serviceType.IsInterface)
            {
                registrationBuilder = registrationBuilder.EnableInterfaceInterceptors();
            }
            else
            {
                (registrationBuilder as IRegistrationBuilder<TLimit, ConcreteReflectionActivatorData, TRegistrationStyle>)?.EnableClassInterceptors();
            }

            foreach (var interceptor in interceptors)
            {
                registrationBuilder.InterceptedBy(interceptor);
            }

            return registrationBuilder;
        }
    }
}
