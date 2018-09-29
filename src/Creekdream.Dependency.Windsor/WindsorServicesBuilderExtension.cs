using Creekdream.Dependency.Windsor;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Windsor specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class WindsorServicesBuilderExtension
    {
        /// <summary>
        /// Use Autofac as an injection container
        /// </summary>
        public static ServicesBuilderOptions UseWindsor(this ServicesBuilderOptions builder)
        {
            builder.IocRegister = new WindsorIocRegister();
            builder.IocRegister.Register<IIocResolver, WindsorIocResolver>(DependencyLifeStyle.Transient);
            return builder;
        }
    }
}

