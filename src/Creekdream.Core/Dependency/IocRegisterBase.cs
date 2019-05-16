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

