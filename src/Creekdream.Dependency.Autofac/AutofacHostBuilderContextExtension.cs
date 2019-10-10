using Autofac;
using Creekdream.Dependency.Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Autofac specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class AutofacHostBuilderContextExtension
    {
        /// <summary>
        /// Use Autofac as an injection container
        /// </summary>
        public static IServiceProviderFactory<ContainerBuilder> UseAutofac(this HostBuilderContext options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return new AutofacServiceProviderFactory();
        }
    }
}

