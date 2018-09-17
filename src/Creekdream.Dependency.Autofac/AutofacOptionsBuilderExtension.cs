using Creekdream.Dependency.Autofac;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Autofac specific extension methods for <see cref="AppOptionsBuilder" />.
    /// </summary>
    public static class AutofacOptionsBuilderExtension
    {
        /// <summary>
        /// Use Autofac as an injection container
        /// </summary>
        public static AppOptionsBuilder UseAutofac(this AppOptionsBuilder builder)
        {
            builder.IocRegister = new AutofacIocRegister();
            builder.IocRegister.Register<IIocResolver, AutofacIocResolver>(DependencyLifeStyle.Transient);
            return builder;
        }
    }
}

