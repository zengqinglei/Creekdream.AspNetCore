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
        public static ServicesBuilderOptions UseAutofac(this ServicesBuilderOptions builder)
        {
            builder.Services.AddSingleton<IocRegisterBase>(new AutofacIocRegister());
            return builder;
        }
    }
}

