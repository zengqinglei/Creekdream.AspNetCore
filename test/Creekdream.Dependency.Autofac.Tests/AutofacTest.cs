using Autofac;
using Autofac.Extras.DynamicProxy;
using Creekdream.Dependency.TestBase.Services;

namespace Creekdream.Dependency.Autofac.Tests
{
    public class AutofacTest : TestBase.TestBase
    {
        protected override IocRegisterBase GetIocRegister()
        {
            var iocRegister = new AutofacIocRegister();
            iocRegister.Register<IIocResolver, AutofacIocResolver>(DependencyLifeStyle.Transient);
            return iocRegister;
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
