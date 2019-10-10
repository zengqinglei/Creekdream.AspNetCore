using Autofac;
using Autofac.Extras.DynamicProxy;
using Creekdream.Dependency.TestBase.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Dependency.Autofac.Tests
{
    public class AutofacTest : TestBase.TestBase
    {
        protected override IServiceCollection GetServices()
        {
            var services = new ServiceCollection();
            var builder = new ContainerBuilder();
            services.AddSingleton((IServiceProviderFactory<ContainerBuilder>)new AutofacServiceProviderFactory());
            return services;
        }

        public override void Test_Register_Assemblies()
        {
            base.Test_Register_Assemblies();
            var builder = new ContainerBuilder();
            builder.RegisterType<ServiceInterceptor>();
            builder.RegisterType<SingletonService>()
                   .As<ISingletonService>()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(ServiceInterceptor));

            var container = builder.Build();
            var singletonService = container.Resolve<ISingletonService>();
            var name = singletonService.GetName();
        }
    }
}
