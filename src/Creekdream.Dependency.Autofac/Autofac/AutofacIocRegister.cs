using Autofac;
using Autofac.Builder;
using System;
//using Autofac.Extras.IocManager.DynamicProxy;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Autofac.Extras.DynamicProxy;
using Autofac.Core.Registration;
using Autofac.Core;

namespace Creekdream.Dependency.Autofac
{
    /// <inheritdoc />
    public class AutofacIocRegister : IocRegisterBase
    {
        private readonly ContainerBuilder _builder;

        /// <inheritdoc />
        public AutofacIocRegister()
        {
            _builder = new ContainerBuilder();
        }

        /// <inheritdoc />
        public override IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            _builder.Populate(services);
            return new AutofacServiceProvider(_builder.Build());
        }

        /// <inheritdoc />
        public override void Register<TService>(TService implementationInstance)
        {
            AddLifeStyle(_builder.Register(c => implementationInstance), DependencyLifeStyle.Singleton);
        }

        /// <inheritdoc />
        public override void Register<TService>(
            Func<IIocResolver, TService> implementationFactory,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            AddLifeStyle(
                _builder.Register(
                    context => implementationFactory.Invoke(context.Resolve<IIocResolver>())
                ),
                lifeStyle);
        }

        /// <inheritdoc />
        public override void Register(Type serviceType, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            if (serviceType.IsGenericType)
            {
                AddLifeStyle(_builder.RegisterGeneric(serviceType), lifeStyle);
            }
            else
            {
                AddLifeStyle(_builder.RegisterType(serviceType), lifeStyle);
            }
        }

        /// <inheritdoc />
        public override void Register(
            Type serviceType,
            Type implementationType,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            if (implementationType.IsGenericType)
            {
                AddLifeStyle(_builder.RegisterGeneric(implementationType).As(serviceType), lifeStyle);
            }
            else
            {
                AddLifeStyle(_builder.RegisterType(implementationType).As(serviceType), lifeStyle);
            }
        }

        /// <inheritdoc />
        public override void RegisterInterceptor<TInterceptor>(Func<TypeInfo, bool> filterCondition)
        {
            _builder.RegisterType<TInterceptor>();
            _builder.RegisterBuildCallback(c =>
            {
                foreach (var registration in c.ComponentRegistry.Registrations)
                {
                    var implType = registration.Activator.LimitType;
                    if (filterCondition(implType.GetTypeInfo()))
                    {
                        var types = registration.Services
                            .OfType<IServiceWithType>()
                            .Select(s => s.ServiceType).ToList();

                        IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb;
                        if (types.Any(t => t.IsClass))
                        {
                            rb = RegistrationBuilder.ForType(types.First(t => t.IsClass)).EnableClassInterceptors();
                            ((ComponentRegistration)registration).Activator = rb.ActivatorData.Activator;
                        }
                        else
                        {
                            rb = RegistrationBuilder.ForType(types.First(t => t.IsInterface)).EnableInterfaceInterceptors();
                        }
                        rb.InterceptedBy(typeof(TInterceptor));

                        foreach (var pair in rb.RegistrationData.Metadata)
                        {
                            registration.Metadata[pair.Key] = pair.Value;
                        }
                        foreach (var p in rb.RegistrationData.PreparingHandlers)
                        {
                            registration.Preparing += p;
                        }
                        foreach (var ac in rb.RegistrationData.ActivatingHandlers)
                        {
                            registration.Activating += ac;
                        }
                        foreach (var ad in rb.RegistrationData.ActivatedHandlers)
                        {
                            registration.Activated += ad;
                        }
                    }
                }
            });
        }

        /// <inheritdoc />
        public override void RegisterAssemblyByBasicInterface(Assembly assembly)
        {
            var transientItem = _builder.RegisterAssemblyTypes(assembly)
                   .Where(type => typeof(IScopedDependency).IsAssignableFrom(type) && !type.IsAbstract)
                   .AsSelf()
                   .AsImplementedInterfaces();
            AddLifeStyle(transientItem, DependencyLifeStyle.Transient);

            var scopedItem = _builder.RegisterAssemblyTypes(assembly)
                   .Where(type => typeof(ITransientDependency).IsAssignableFrom(type) && !type.IsAbstract)
                   .AsSelf()
                   .AsImplementedInterfaces();
            AddLifeStyle(scopedItem, DependencyLifeStyle.Scoped);

            var singletonItem = _builder.RegisterAssemblyTypes(assembly)
                   .Where(type => typeof(ISingletonDependency).IsAssignableFrom(type) && !type.IsAbstract)
                   .AsSelf()
                   .AsImplementedInterfaces();
            AddLifeStyle(singletonItem, DependencyLifeStyle.Singleton);
        }

        /// <summary>
        /// Set the lifestyle of the registered builder
        /// </summary>
        private void AddLifeStyle<TLimit, TActiatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActiatorData, TRegistrationStyle> item,
            DependencyLifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    item.InstancePerDependency();
                    break;
                case DependencyLifeStyle.Scoped:
                    item.InstancePerLifetimeScope();
                    break;
                case DependencyLifeStyle.Singleton:
                    item.SingleInstance();
                    break;
                default:
                    throw new ArgumentException(nameof(lifeStyle));
            }
        }
    }
}

