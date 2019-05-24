using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Creekdream.Dependency.Autofac
{
    /// <summary>
    /// Service factory based on Autofac
    /// </summary>
    public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly ContainerBuilder _builder;

        /// <inheritdoc />
        public AutofacServiceProviderFactory(ContainerBuilder builder)
        {
            _builder = builder;
        }


        /// <inheritdoc />
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            _builder.Populate(services);

            return _builder;
        }

        /// <inheritdoc />
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            return new AutofacServiceProvider(containerBuilder.Build());
        }

        private void RegisterInterceptors(IServiceCollection services)
        {
        }

        private void RegisterServices(IServiceCollection services)
        {
            
        }
    }
}
