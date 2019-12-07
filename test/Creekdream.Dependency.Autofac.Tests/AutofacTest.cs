using Autofac;
using Autofac.Extras.DynamicProxy;
using Creekdream.Dependency.TestBase.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using Xunit;

namespace Creekdream.Dependency.Autofac.Tests
{
    public class AutofacTest : TestBase.TestBase
    {
        protected override IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            var factory = new AutofacServiceProviderFactory();
            var builder = factory.CreateBuilder(services);
            return factory.CreateServiceProvider(builder);
        }

        [Fact]
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
            name.ShouldBeNull();
        }
    }
}
