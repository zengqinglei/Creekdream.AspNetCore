using Creekdream.AspNetCore;
using Creekdream.Dependency.TestBase.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using Xunit;

namespace Creekdream.Dependency.TestBase
{
    public abstract class TestBase
    {
        protected virtual IServiceCollection GetServices()
        {
            return new ServiceCollection();
        }

        protected abstract IServiceProvider GetServiceProvider(IServiceCollection services);

        [Fact]
        public void Test_Register_SingleInstance()
        {
            var services = GetServices();
            services.AddSingleton(new ServiceOptions());

            var serviceProvider = GetServiceProvider(services);

            var serviceOptions1 = serviceProvider.GetService<ServiceOptions>();
            serviceOptions1.ShouldNotBeNull();
            var serviceOptions2 = serviceProvider.GetService<ServiceOptions>();
            serviceOptions2.ShouldNotBeNull();

            serviceOptions1.Id.ShouldBe(serviceOptions2.Id);
        }

        [Fact]
        public void Test_Register_Services()
        {
            var services = GetServices();
            services.AddSingleton<SingletonService>();
            services.AddScoped<ScopedService>();
            services.AddTransient<TransientService>();

            var serviceProvider = GetServiceProvider(services);

            var singletonService1 = serviceProvider.GetService<SingletonService>();
            singletonService1.ShouldNotBeNull();
            var singletonService2 = serviceProvider.GetService<SingletonService>();
            singletonService2.ShouldNotBeNull();
            singletonService1.Id.ShouldBe(singletonService2.Id);

            var scopedService1 = serviceProvider.GetService<ScopedService>();
            scopedService1.ShouldNotBeNull();
            var scopedService2 = serviceProvider.GetService<ScopedService>();
            scopedService2.ShouldNotBeNull();
            scopedService1.Id.ShouldBe(scopedService2.Id);

            var transientService1 = serviceProvider.GetService<TransientService>();
            transientService1.ShouldNotBeNull();
            var transientService2 = serviceProvider.GetService<TransientService>();
            transientService2.ShouldNotBeNull();
            transientService1.Id.ShouldNotBe(transientService2.Id);
        }

        [Fact]
        public virtual void Test_Register_ServicesFactory()
        {
            var services = GetServices();
            services.AddSingleton(
                provider =>
                {
                    return new SingletonService();
                });
            services.AddScoped(
                provider =>
                {
                    return new ScopedService();
                });
            services.AddTransient(
                provider =>
                {
                    return new TransientService();
                });

            var serviceProvider = GetServiceProvider(services);

            var singletonService1 = serviceProvider.GetService<SingletonService>();
            singletonService1.ShouldNotBeNull();
            var singletonService2 = serviceProvider.GetService<SingletonService>();
            singletonService2.ShouldNotBeNull();
            singletonService1.Id.ShouldBe(singletonService2.Id);

            var scopedService1 = serviceProvider.GetService<ScopedService>();
            scopedService1.ShouldNotBeNull();
            var scopedService2 = serviceProvider.GetService<ScopedService>();
            scopedService2.ShouldNotBeNull();
            scopedService1.Id.ShouldBe(scopedService2.Id);

            var transientService1 = serviceProvider.GetService<TransientService>();
            transientService1.ShouldNotBeNull();
            var transientService2 = serviceProvider.GetService<TransientService>();
            transientService2.ShouldNotBeNull();
            transientService1.Id.ShouldNotBe(transientService2.Id);
        }

        [Fact]
        public void Test_Register_ServicesAndImplementation()
        {
            var services = GetServices();
            services.AddSingleton<ISingletonService, SingletonService>();
            services.AddScoped<IScopedService, ScopedService>();
            services.AddTransient<ITransientService, TransientService>();

            var serviceProvider = GetServiceProvider(services);

            var singletonService1 = serviceProvider.GetService<ISingletonService>();
            singletonService1.ShouldNotBeNull();
            var singletonService2 = serviceProvider.GetService<ISingletonService>();
            singletonService2.ShouldNotBeNull();
            singletonService1.Id.ShouldBe(singletonService2.Id);

            var scopedService1 = serviceProvider.GetService<IScopedService>();
            scopedService1.ShouldNotBeNull();
            var scopedService2 = serviceProvider.GetService<IScopedService>();
            scopedService2.ShouldNotBeNull();
            scopedService1.Id.ShouldBe(scopedService2.Id);

            var transientService1 = serviceProvider.GetService<ITransientService>();
            transientService1.ShouldNotBeNull();
            var transientService2 = serviceProvider.GetService<ITransientService>();
            transientService2.ShouldNotBeNull();
            transientService1.Id.ShouldNotBe(transientService2.Id);
        }

        [Fact]
        public void Test_Register_Interceptor()
        {
            var services = GetServices();
            services.AddSingleton<ServiceInterceptor>();
            services.OnRegistred(
                context =>
                {
                    if (
                        typeof(ISingletonService).IsAssignableFrom(context.ImplementationType) ||
                        typeof(IScopedService).IsAssignableFrom(context.ImplementationType) ||
                        typeof(ITransientService).IsAssignableFrom(context.ImplementationType)
                    )
                    {
                        context.Interceptors.Add<ServiceInterceptor>();
                    }
                });
            services.AddSingleton<ISingletonService, SingletonService>();
            services.AddScoped<IScopedService, ScopedService>();
            services.AddTransient<ITransientService, TransientService>();

            var serviceProvider = GetServiceProvider(services);

            var singletonService = serviceProvider.GetService<ISingletonService>();
            var singletonServiceName = singletonService.GetName();
            SingletonService.Interceptor.ShouldBe("Singleton");

            var scopedService = serviceProvider.GetService<IScopedService>();
            var scopedServiceName = scopedService.GetName();
            ScopedService.Interceptor.ShouldBe("Scoped");

            var transientService = serviceProvider.GetService<ITransientService>();
            var transientServiceName = transientService.GetName();
            TransientService.Interceptor.ShouldBe("Transient");
        }

        [Fact]
        public virtual void Test_Register_Assemblies()
        {
            var services = GetServices();
            services.AddSingleton<ServiceInterceptor>();
            services.RegisterAssemblyByBasicInterface(typeof(TestBase).Assembly);
            services.OnRegistred(
                context =>
                {
                    if (
                        typeof(ISingletonService).IsAssignableFrom(context.ImplementationType) ||
                        typeof(IScopedService).IsAssignableFrom(context.ImplementationType) ||
                        typeof(ITransientService).IsAssignableFrom(context.ImplementationType)
                    )
                    {
                        context.Interceptors.Add<ServiceInterceptor>();
                    }
                });

            var serviceProvider = GetServiceProvider(services);

            var serviceOptions = serviceProvider.GetService<ServiceOptions>();
            serviceOptions.ShouldNotBeNull();

            var singletonService = serviceProvider.GetService<ISingletonService>();
            var singletonServiceName = singletonService.GetName();
            singletonService.ShouldNotBeNull();

            var scopedService = serviceProvider.GetService<IScopedService>();
            scopedService.ShouldNotBeNull();

            var transientService = serviceProvider.GetService<ITransientService>();
            transientService.ShouldNotBeNull();
        }
    }
}
