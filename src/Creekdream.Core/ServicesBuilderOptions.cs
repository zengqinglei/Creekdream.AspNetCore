using Creekdream.Dependency;
using Creekdream.Uow;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Creekdream
{
    /// <summary>
    /// Application framework initialization
    /// </summary>
    public class ServicesBuilderOptions
    {
        /// <summary>
        /// services
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Uow options
        /// </summary>
        public UnitOfWorkOptions UowOptions { get; set; }

        /// <inheritdoc />
        public ServicesBuilderOptions(IServiceCollection services)
        {
            Services = services;
            UowOptions = new UnitOfWorkOptions();
        }

        /// <summary>
        /// Build and initialize
        /// </summary>
        public IServiceProvider Build()
        {
            Services.AddSingleton(UowOptions);
            Services.AddSingleton(this);
            Services.RegisterInterceptor<UnitOfWorkInterceptor>(
                implementationType =>
                {
                    if (implementationType.IsDefined(typeof(UnitOfWorkAttribute), true))
                    {
                        return true;
                    }
                    var methods = implementationType.GetMethods(
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);
                    if (methods.Any(m => m.IsDefined(typeof(UnitOfWorkAttribute), true)))
                    {
                        return true;
                    }
                    if (UowOptions.ConventionalUowSelectors.Any(selector => selector(implementationType)))
                    {
                        return true;
                    }
                    return false;
                });
            Services.RegisterAssemblyByBasicInterface(GetType().Assembly);
            return Services.GetServiceProvider();
        }
    }
}
