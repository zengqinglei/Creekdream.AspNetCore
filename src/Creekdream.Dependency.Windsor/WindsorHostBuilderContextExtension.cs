using Castle.Windsor;
using Creekdream.Dependency.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Windsor specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class WindsorHostBuilderContextExtension
    {
        /// <summary>
        /// Use Autofac as an injection container
        /// </summary>
        public static IServiceProviderFactory<IWindsorContainer> UseWindsor(this HostBuilderContext options)
        {
            var container = new WindsorContainer();
            return new WindsorServiceProviderFactory(container);
        }
    }
}

