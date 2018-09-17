using Castle.Windsor;
using System;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Castle.Core;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Dependency.Windsor
{
    /// <inheritdoc />
    public class WindsorIocRegister : IocRegisterBase
    {
        private readonly IWindsorContainer _container;

        /// <inheritdoc />
        public WindsorIocRegister()
        {
            _container = new WindsorContainer();
        }

        /// <inheritdoc />
        public override IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            services.AddSingleton(_container);
            return WindsorRegistrationHelper.CreateServiceProvider(_container, services);
        }

        /// <inheritdoc />
        public override void Register<TService>(TService implementationInstance)
        {
            _container.Register(ApplyLifestyle(Component.For().Instance(implementationInstance), DependencyLifeStyle.Singleton));
        }

        /// <inheritdoc />
        public override void Register(Type serviceType, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            _container.Register(ApplyLifestyle(Component.For(serviceType), lifeStyle));
        }

        /// <inheritdoc />
        public override void Register(
            Type serviceType,
            Func<IIocResolver, object> implementationFactory,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            _container.Register(
                ApplyLifestyle(
                    Component.For(serviceType).UsingFactoryMethod(
                        serviceLocator => implementationFactory.Invoke(serviceLocator.Resolve<IIocResolver>())),
                    lifeStyle
                )
            );
        }

        /// <inheritdoc />
        public override void Register(
            Type serviceType,
            Type implementationType,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            _container.Register(ApplyLifestyle(Component.For(serviceType, implementationType).ImplementedBy(implementationType), lifeStyle));
        }

        /// <inheritdoc />
        public override void RegisterInterceptor<TInterceptor>(Func<TypeInfo, bool> filterCondition)
        {
            _container.Register(Component.For<TInterceptor>().LifestyleSingleton());
            _container.Kernel.ComponentRegistered +=
                (key, handler) =>
                {
                    var implType = handler.ComponentModel.Implementation.GetTypeInfo();
                    if (filterCondition(implType))
                    {
                        handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(TInterceptor)));
                    }
                };
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
                    .LifestyleScoped()
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

        /// <summary>
        /// Set the lifestyle of the registered builder
        /// </summary>
        private ComponentRegistration<T> ApplyLifestyle<T>(ComponentRegistration<T> registration, DependencyLifeStyle lifeStyle)
            where T : class
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    return registration.LifestyleTransient();
                case DependencyLifeStyle.Scoped:
                    return registration.LifestyleScoped();
                case DependencyLifeStyle.Singleton:
                    return registration.LifestyleSingleton();
                default:
                    throw new ArgumentException(nameof(lifeStyle));
            }
        }
    }
}

