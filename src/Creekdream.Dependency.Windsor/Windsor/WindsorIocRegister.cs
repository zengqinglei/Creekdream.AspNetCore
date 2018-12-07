using Castle.Windsor;
using System;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Castle.Core;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Castle.MicroKernel;
using System.Collections.Generic;
using System.Linq;

namespace Creekdream.Dependency.Windsor
{
    /// <inheritdoc />
    public class WindsorIocRegister : IocRegisterBase
    {
        private readonly IWindsorContainer _container;
        private readonly List<Action<List<IHandler>>> _componentRegistedEvents;

        /// <inheritdoc />
        public WindsorIocRegister()
        {
            _container = new WindsorContainer();
            _componentRegistedEvents = new List<Action<List<IHandler>>>();
        }

        /// <inheritdoc />
        public override IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            var handlers = _container.Kernel.GetAssignableHandlers(typeof(object));
            foreach (var componentRegistedEvent in _componentRegistedEvents)
            {
                componentRegistedEvent.Invoke(handlers.ToList());
            }

            services.AddSingleton(_container);
            return WindsorRegistrationHelper.CreateServiceProvider(_container, services);
        }

        /// <inheritdoc />
        public override void Register<TService>(TService implementationInstance)
        {
            _container.Register(Component.For<TService>().Instance(implementationInstance).ApplyLifestyle(DependencyLifeStyle.Singleton));
        }

        /// <inheritdoc />
        public override void Register(Type serviceType, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            _container.Register(Component.For(serviceType).ApplyLifestyle(lifeStyle));
        }

        /// <inheritdoc />
        public override void Register<TService>(
            Func<IIocResolver, TService> implementationFactory,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            _container.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(kernel => implementationFactory.Invoke(kernel.Resolve<IIocResolver>()))
                    .ApplyLifestyle(lifeStyle)
            );
        }

        /// <inheritdoc />
        public override void Register(
            Type serviceType,
            Type implementationType,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            _container.Register(Component.For(serviceType, implementationType).ImplementedBy(implementationType).ApplyLifestyle(lifeStyle));
        }

        /// <inheritdoc />
        public override void RegisterInterceptor<TInterceptor>(Func<TypeInfo, bool> filterCondition)
        {
            _container.Register(Component.For<TInterceptor>().LifestyleTransient());
            _componentRegistedEvents.Add(
                handlers =>
                {
                    foreach (var handler in handlers)
                    {
                        var implType = handler.ComponentModel.Implementation.GetTypeInfo();
                        if (filterCondition(implType))
                        {
                            handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(TInterceptor)));
                        }
                    }
                });
        }

        /// <inheritdoc />
        public override void RegisterAssemblyByBasicInterface(Assembly assembly)
        {
            _container.Register(
                Classes.FromAssembly(assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<IScopedDependency>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleScoped<LocalLifetimeScopeAccessor>()
                );

            _container.Register(
                Classes.FromAssembly(assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<ITransientDependency>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient()
                );

            _container.Register(
                Classes.FromAssembly(assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<ISingletonDependency>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleSingleton()
                );
        }
    }
}

