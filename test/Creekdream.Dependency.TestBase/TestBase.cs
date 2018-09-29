using Creekdream.Dependency.TestBase.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Creekdream.Dependency.TestBase
{
    public abstract class TestBase
    {
        protected abstract IocRegisterBase GetIocRegister();

        protected IIocResolver GetIocResolver(IocRegisterBase iocRegister)
        {
            var serviceProvider = iocRegister.GetServiceProvider(new ServiceCollection());
            return serviceProvider.GetService<IIocResolver>();
        }

        [Fact]
        public void Test_Register_SingleInstance()
        {
            var iocRegister = GetIocRegister();
            iocRegister.Register(new ServiceOptions());

            var iocResolver = GetIocResolver(iocRegister);

            var serviceOptions1 = iocResolver.Resolve<ServiceOptions>();
            serviceOptions1.ShouldNotBeNull();
            var serviceOptions2 = iocResolver.Resolve<ServiceOptions>();
            serviceOptions2.ShouldNotBeNull();

            serviceOptions1.Id.ShouldBe(serviceOptions2.Id);
        }

        [Fact]
        public void Test_Register_Services()
        {
            var iocRegister = GetIocRegister();
            iocRegister.Register<SingletonService>(DependencyLifeStyle.Singleton);
            iocRegister.Register<ScopedService>(DependencyLifeStyle.Scoped);
            iocRegister.Register<TransientService>(DependencyLifeStyle.Transient);

            var iocResolver = GetIocResolver(iocRegister);

            var singletonService1 = iocResolver.Resolve<SingletonService>();
            singletonService1.ShouldNotBeNull();
            var singletonService2 = iocResolver.Resolve<SingletonService>();
            singletonService2.ShouldNotBeNull();
            singletonService1.Id.ShouldBe(singletonService2.Id);

            var scopedService1 = iocResolver.Resolve<ScopedService>();
            scopedService1.ShouldNotBeNull();
            var scopedService2 = iocResolver.Resolve<ScopedService>();
            scopedService2.ShouldNotBeNull();
            scopedService1.Id.ShouldBe(scopedService2.Id);

            var transientService1 = iocResolver.Resolve<TransientService>();
            transientService1.ShouldNotBeNull();
            var transientService2 = iocResolver.Resolve<TransientService>();
            transientService2.ShouldNotBeNull();
            transientService1.Id.ShouldNotBe(transientService2.Id);
        }

        [Fact]
        public virtual void Test_Register_ServicesFactory()
        {
            var iocRegister = GetIocRegister();
            iocRegister.Register(
                resolver =>
                {
                    return new SingletonService();
                }, DependencyLifeStyle.Singleton);
            iocRegister.Register(
                resolver =>
                {
                    return new ScopedService();
                }, DependencyLifeStyle.Scoped);
            iocRegister.Register(
                resolver =>
                {
                    return new TransientService();
                }, DependencyLifeStyle.Transient);

            var iocResolver = GetIocResolver(iocRegister);

            var singletonService1 = iocResolver.Resolve<SingletonService>();
            singletonService1.ShouldNotBeNull();
            var singletonService2 = iocResolver.Resolve<SingletonService>();
            singletonService2.ShouldNotBeNull();
            singletonService1.Id.ShouldBe(singletonService2.Id);

            var scopedService1 = iocResolver.Resolve<ScopedService>();
            scopedService1.ShouldNotBeNull();
            var scopedService2 = iocResolver.Resolve<ScopedService>();
            scopedService2.ShouldNotBeNull();
            scopedService1.Id.ShouldBe(scopedService2.Id);

            var transientService1 = iocResolver.Resolve<TransientService>();
            transientService1.ShouldNotBeNull();
            var transientService2 = iocResolver.Resolve<TransientService>();
            transientService2.ShouldNotBeNull();
            transientService1.Id.ShouldNotBe(transientService2.Id);
        }

        [Fact]
        public void Test_Register_ServicesAndImplementation()
        {
            var iocRegister = GetIocRegister();
            iocRegister.Register<ISingletonService, SingletonService>(DependencyLifeStyle.Singleton);
            iocRegister.Register<IScopedService, ScopedService>(DependencyLifeStyle.Scoped);
            iocRegister.Register<ITransientService, TransientService>(DependencyLifeStyle.Transient);

            var iocResolver = GetIocResolver(iocRegister);

            var singletonService1 = iocResolver.Resolve<ISingletonService>();
            singletonService1.ShouldNotBeNull();
            var singletonService2 = iocResolver.Resolve<ISingletonService>();
            singletonService2.ShouldNotBeNull();
            singletonService1.Id.ShouldBe(singletonService2.Id);

            var scopedService1 = iocResolver.Resolve<IScopedService>();
            scopedService1.ShouldNotBeNull();
            var scopedService2 = iocResolver.Resolve<IScopedService>();
            scopedService2.ShouldNotBeNull();
            scopedService1.Id.ShouldBe(scopedService2.Id);

            var transientService1 = iocResolver.Resolve<ITransientService>();
            transientService1.ShouldNotBeNull();
            var transientService2 = iocResolver.Resolve<ITransientService>();
            transientService2.ShouldNotBeNull();
            transientService1.Id.ShouldNotBe(transientService2.Id);
        }

        [Fact]
        public void Test_Register_Interceptor()
        {
            var iocRegister = GetIocRegister();
            iocRegister.RegisterInterceptor<ServiceInterceptor>(
                implementationType =>
                {
                    if (typeof(ISingletonService).IsAssignableFrom(implementationType))
                    {
                        return true;
                    }
                    if (typeof(IScopedService).IsAssignableFrom(implementationType))
                    {
                        return true;
                    }
                    if (typeof(ITransientService).IsAssignableFrom(implementationType))
                    {
                        return true;
                    }
                    return false;
                });
            iocRegister.Register<ISingletonService, SingletonService>(DependencyLifeStyle.Singleton);
            iocRegister.Register<IScopedService, ScopedService>(DependencyLifeStyle.Scoped);
            iocRegister.Register<ITransientService, TransientService>(DependencyLifeStyle.Transient);

            var iocResolver = GetIocResolver(iocRegister);

            var singletonService = iocResolver.Resolve<ISingletonService>();
            var singletonServiceName = singletonService.GetName();
            SingletonService.Interceptor.ShouldBe("Singleton");

            var scopedService = iocResolver.Resolve<IScopedService>();
            var scopedServiceName = scopedService.GetName();
            ScopedService.Interceptor.ShouldBe("Scoped");

            var transientService = iocResolver.Resolve<ITransientService>();
            var transientServiceName = transientService.GetName();
            TransientService.Interceptor.ShouldBe("Transient");
        }

        [Fact]
        public virtual void Test_Register_Assemblies()
        {
            var iocRegister = GetIocRegister();
            iocRegister.RegisterAssemblyByBasicInterface(typeof(TestBase).Assembly);
            iocRegister.RegisterInterceptor<ServiceInterceptor>(
                implementationType =>
                {
                    if (typeof(ISingletonService).IsAssignableFrom(implementationType))
                    {
                        return true;
                    }
                    if (typeof(IScopedService).IsAssignableFrom(implementationType))
                    {
                        return true;
                    }
                    if (typeof(ITransientService).IsAssignableFrom(implementationType))
                    {
                        return true;
                    }
                    return false;
                });

            var iocResolver = GetIocResolver(iocRegister);

            var serviceOptions = iocResolver.Resolve<ServiceOptions>();
            serviceOptions.ShouldNotBeNull();

            var singletonService = iocResolver.Resolve<ISingletonService>();
            var singletonServiceName = singletonService.GetName();
            singletonService.ShouldNotBeNull();

            var scopedService = iocResolver.Resolve<IScopedService>();
            scopedService.ShouldNotBeNull();

            var transientService = iocResolver.Resolve<ITransientService>();
            transientService.ShouldNotBeNull();
        }
    }
}
