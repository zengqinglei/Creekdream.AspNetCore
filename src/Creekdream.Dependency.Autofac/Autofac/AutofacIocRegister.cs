using Autofac;
using Autofac.Builder;
using System;
using Autofac.Extras.IocManager.DynamicProxy;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Autofac.Extras.DynamicProxy;
using Autofac.Core.Registration;

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
        public override void Register(
            Type serviceType,
            Func<IIocResolver, object> implementationFactory,
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
            _builder.RegisterCallback((c) =>
            {
                c.Registered += (sender, args) =>
                {
                    var implType = args.ComponentRegistration.Activator.LimitType;
                    if (filterCondition(implType.GetTypeInfo()))
                    {
                        //args.ComponentRegistration.Activating += (s, e) =>
                        //{
                        //    var proxy = new ProxyGenerator().CreateClassProxyWithTarget(
                        //        e.Instance.GetType(),
                        //        e.Instance,
                        //        e.Context.Resolve<IEnumerable<IInterceptor>>().ToArray());

                        //    e.ReplaceInstance(proxy);
                        //};

                        var registration = args.ComponentRegistration;

                        var rb = RegistrationBuilder.ForType(implType);
                        rb.EnableInterfaceInterceptors().InterceptedBy(typeof(TInterceptor));
                        ((ComponentRegistration)registration).Activator = rb.ActivatorData.Activator;

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
                        //args.ComponentRegistration.InterceptedBy(typeof(TInterceptor));
                    }
                };
            });
        }

        /// <inheritdoc />
        public override void RegisterAssemblyByBasicInterface(Assembly assembly)
        {
            var transientItem = _builder.RegisterAssemblyTypes(assembly)
                   .Where(type => typeof(IScopedDependency).IsAssignableFrom(type) && !type.IsAbstract)
                   .AsImplementedInterfaces();
            AddLifeStyle(transientItem, DependencyLifeStyle.Transient);

            var scopedItem = _builder.RegisterAssemblyTypes(assembly)
                   .Where(type => typeof(ITransientDependency).IsAssignableFrom(type) && !type.IsAbstract)
                   .AsImplementedInterfaces();
            AddLifeStyle(scopedItem, DependencyLifeStyle.Scoped);

            var singletonItem = _builder.RegisterAssemblyTypes(assembly)
                   .Where(type => typeof(ISingletonDependency).IsAssignableFrom(type) && !type.IsAbstract)
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

