using Creekdream.Dependency.Autofac;

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
            builder.IocRegister = new AutofacIocRegister();
            builder.IocRegister.Register<IIocResolver, AutofacIocResolver>(DependencyLifeStyle.Transient);
            return builder;
        }
    }
}

