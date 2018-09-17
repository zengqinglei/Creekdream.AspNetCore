using Creekdream.Dependency.Windsor;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Windsor specific extension methods for <see cref="AppOptionsBuilder" />.
    /// </summary>
    public static class WindsorOptionsBuilderExtension
    {
        /// <summary>
        /// Use Autofac as an injection container
        /// </summary>
        public static AppOptionsBuilder UseWindsor(this AppOptionsBuilder builder)
        {
            builder.IocRegister = new WindsorIocRegister();
            builder.IocRegister.Register<IIocResolver, WindsorIocResolver>(DependencyLifeStyle.Transient);
            return builder;
        }
    }
}

