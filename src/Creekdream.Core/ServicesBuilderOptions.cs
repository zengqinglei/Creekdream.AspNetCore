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
        /// container builder type
        /// </summary>
        public Type ServiceProviderFactoryType { get; set; }

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
        public void Initialize()
        {
            Services.AddSingleton(UowOptions);
            Services.AddSingleton(this);
            Services.AddSingleton<UnitOfWorkInterceptor>();
            Services.OnRegistred(context =>
            {
                if (context.ImplementationType.IsDefined(typeof(UnitOfWorkAttribute), true))
                {
                    context.Interceptors.Add<UnitOfWorkInterceptor>();
                    return;
                }
                var methods = context.ImplementationType.GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);
                if (methods.Any(m => m.IsDefined(typeof(UnitOfWorkAttribute), true)))
                {
                    context.Interceptors.Add<UnitOfWorkInterceptor>();
                    return;
                }
                if (UowOptions.ConventionalUowSelectors.Any(selector => selector(context.ImplementationType)))
                {
                    context.Interceptors.Add<UnitOfWorkInterceptor>();
                    return;
                }
            });
            Services.RegisterAssemblyByBasicInterface(GetType().Assembly);
        }
    }
}
