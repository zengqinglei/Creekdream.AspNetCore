using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream.Dependency.Windsor.Tests
{
    public class WindsorTest : TestBase.TestBase
    {
        protected override IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            var container = new WindsorContainer();
            var factory = new WindsorServiceProviderFactory(container);
            var builder = factory.CreateBuilder(services);
            return factory.CreateServiceProvider(builder);
        }
    }
}
