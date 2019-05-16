using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Dependency.Windsor.Tests
{
    public class WindsorTest : TestBase.TestBase
    {
        protected override IServiceCollection GetServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IocRegisterBase, WindsorIocRegister>();
            return services;
        }
    }
}
