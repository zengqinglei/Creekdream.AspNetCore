using Autofac;
using Creekdream.Dependency.Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Autofac specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class AutofacServicesBuilderExtension
    {
        /// <summary>
        /// Use Autofac as an injection container
        /// </summary>
        public static ServicesBuilderOptions UseAutofac(this ServicesBuilderOptions options)
        {
            var builder = new ContainerBuilder();
            options.Services.AddSingleton<IServiceProviderFactory<ContainerBuilder>>(new AutofacServiceProviderFactory(builder));
            return options;
        }
    }
}

