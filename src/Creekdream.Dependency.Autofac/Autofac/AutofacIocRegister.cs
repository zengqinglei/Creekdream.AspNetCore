using Autofac;
using Autofac.Builder;
using System;
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
            IContainer container = null;
            _builder.Populate(services);
            _builder.Register(c => container).SingleInstance();
            container = _builder.Build();
            return new AutofacServiceProvider(container);
        }

        /// <inheritdoc />
        public override void RegisterInterceptor<TInterceptor>(Func<TypeInfo, bool> filterCondition)
        {
            _builder.RegisterType<TInterceptor>().AddLifeStyle(DependencyLifeStyle.Transient);
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
                            rb = RegistrationBuilder.ForType(implType).EnableClassInterceptors();
                        }
                        else
                        {
                            rb = RegistrationBuilder.ForType(implType).AsImplementedInterfaces().EnableInterfaceInterceptors();
                            ((ComponentRegistration)registration).Activator = rb.ActivatorData.Activator;
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
            var assemblyTypes = assembly.GetTypes();

            var transientTypes = assemblyTypes
                .Where(type =>
                    type.GetInterfaces().Any(i => i.GetTypeInfo() == typeof(ITransientDependency)) &&
                    !type.IsAbstract &&
                    !type.GetTypeInfo().IsGenericTypeDefinition)
                .ToArray();
            foreach (var transientType in transientTypes)
            {
                _builder.RegisterType(transientType).AsImplementedInterfaces().AddLifeStyle(DependencyLifeStyle.Transient);
                _builder.RegisterType(transientType).AddLifeStyle(DependencyLifeStyle.Transient);
            }

            var scopedTypes = assemblyTypes
                .Where(type =>
                    type.GetInterfaces().Any(i => i.GetTypeInfo() == typeof(IScopedDependency)) &&
                    !type.IsAbstract &&
                    !type.GetTypeInfo().IsGenericTypeDefinition)
                .ToArray();
            foreach (var scopedType in scopedTypes)
            {
                _builder.RegisterType(scopedType).AsImplementedInterfaces().AddLifeStyle(DependencyLifeStyle.Scoped);
                _builder.RegisterType(scopedType).AddLifeStyle(DependencyLifeStyle.Scoped);
            }

            var singletonTypes = assemblyTypes
                .Where(type =>
                    type.GetInterfaces().Any(i => i.GetTypeInfo() == typeof(ISingletonDependency)) &&
                    !type.IsAbstract &&
                    !type.GetTypeInfo().IsGenericTypeDefinition)
                .ToArray();
            foreach (var singletonType in singletonTypes)
            {
                _builder.RegisterType(singletonType).AsImplementedInterfaces().AddLifeStyle(DependencyLifeStyle.Singleton);
                _builder.RegisterType(singletonType).AddLifeStyle(DependencyLifeStyle.Singleton);
            }
        }
    }
}

