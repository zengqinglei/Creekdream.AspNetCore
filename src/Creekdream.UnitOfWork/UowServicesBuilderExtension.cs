using Creekdream.Uow;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Creekdream.Dependency
{
    /// <summary>
    /// UnitOfUork specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class UowServicesBuilderExtension
    {
        /// <summary>
        /// Use unit of work
        /// </summary>
        public static ServicesBuilderOptions UseUnitOfWork(this ServicesBuilderOptions options, Action<UnitOfWorkOptions> uowOptions = null)
        {
            var defaultUowOptins = new UnitOfWorkOptions();
            uowOptions?.Invoke(defaultUowOptins);
            options.Services.AddSingleton(defaultUowOptins);
            options.Services.OnRegistred(context =>
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
                if (defaultUowOptins.ConventionalUowSelectors.Any(selector => selector(context.ImplementationType)))
                {
                    context.Interceptors.Add<UnitOfWorkInterceptor>();
                    return;
                }
            });
            options.Services.RegisterAssemblyByBasicInterface(typeof(UowServicesBuilderExtension).Assembly);
            return options;
        }
    }
}

