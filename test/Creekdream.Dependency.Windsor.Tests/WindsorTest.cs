using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Dependency.Windsor.Tests
{
    public class WindsorTest : TestBase.TestBase
    {
        protected override IServiceCollection GetServices()
        {
            var services = new ServiceCollection();
            var container = new WindsorContainer();
            services.AddSingleton((IServiceProviderFactory<IWindsorContainer>)new WindsorServiceProviderFactory(container));
            return services;
        }
    }
}
