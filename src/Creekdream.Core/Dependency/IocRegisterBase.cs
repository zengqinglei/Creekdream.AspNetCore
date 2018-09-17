using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Defining a registration abstract class
    /// </summary>
    public abstract class IocRegisterBase
    {
        /// <summary>
        /// Get service provider
        /// </summary>
        public abstract IServiceProvider GetServiceProvider(IServiceCollection services);

        /// <summary>
        /// Register an instance to a service generic
        /// </summary>
        public abstract void Register<TService>(TService implementationInstance)
            where TService : class;

        /// <summary>
        /// Register service generic
        /// </summary>
        public virtual void Register<TService>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TService : class
        {
            Register(typeof(TService), lifeStyle: lifeStyle);
        }

        /// <summary>
        /// Register service type
        /// </summary>
        public abstract void Register(Type serviceType, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton);

        /// <summary>
        /// Register an instance obtained through the factory to service generics
        /// </summary>
        public virtual void Register<TService>(
            Func<IIocResolver, object> implementationFactory,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TService : class
        {
            Register(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Register an instance obtained through the factory to service type
        /// </summary>
        public abstract void Register(
            Type serviceType,
            Func<IIocResolver, object> implementationFactory,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton);

        /// <summary>
        /// Register an instance obtained through the factory to service generics
        /// </summary>
        public virtual void Register<TService>(
            Func<IIocResolver, TService> implementationFactory,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TService : class
        {
            Register(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Register an instance obtained through the factory to service generics
        /// </summary>
        public virtual void Register<TService, TImplementation>(
            Func<IIocResolver, TImplementation> implementationFactory,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TService : class
            where TImplementation : class, TService
        {
            Register(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Register implementation to service interfaces
        /// </summary>
        public virtual void Register<TService, TImplementation>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TService : class
            where TImplementation : class, TService
        {
            Register(typeof(TService), typeof(TImplementation), lifeStyle: lifeStyle);
        }

        /// <summary>
        /// Register implementation to service interfaces
        /// </summary>
        public abstract void Register(
            Type serviceType,
            Type implementationType,
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton);

        /// <summary>
        /// Register interceptor
        /// </summary>
        public abstract void RegisterInterceptor<TInterceptor>(Func<TypeInfo, bool> filterCondition)
            where TInterceptor : InterceptorBase;

        /// <summary>
        /// General interface implementation in the registration assembly
        /// </summary>
        public abstract void RegisterAssemblyByBasicInterface(Assembly assembly);
    }
}

