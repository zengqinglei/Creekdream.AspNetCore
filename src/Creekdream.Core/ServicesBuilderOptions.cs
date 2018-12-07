using Creekdream.Application.Service;
using Creekdream.Dependency;
using Creekdream.Domain.Repositories;
using Creekdream.Uow;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Creekdream
{
    /// <summary>
    /// Application framework initialization
    /// </summary>
    public class ServicesBuilderOptions
    {
        /// <summary>
        /// Ioc register
        /// </summary>
        public IocRegisterBase IocRegister { get; set; }

        /// <summary>
        /// Uow options
        /// </summary>
        public UnitOfWorkOptions UowOptions { get; set; }

        /// <inheritdoc />
        public ServicesBuilderOptions()
        {
            UowOptions = new UnitOfWorkOptions();
        }

        /// <summary>
        /// Build and initialize
        /// </summary>
        public IServiceProvider Build(IServiceCollection services)
        {
            IocRegister.Register(UowOptions);
            IocRegister.Register(this);
            IocRegister.RegisterInterceptor<UnitOfWorkInterceptor>(
                implementationType =>
                {
                    if (implementationType.IsDefined(typeof(UnitOfWorkAttribute), true))
                    {
                        return true;
                    }
                    var methods = implementationType.GetMethods(
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);
                    if (methods.Any(m => m.IsDefined(typeof(UnitOfWorkAttribute), true)))
                    {
                        return true;
                    }
                    if (UowOptions.ConventionalUowSelectors.Any(selector => selector(implementationType)))
                    {
                        return true;
                    }
                    return false;
                });
            IocRegister.RegisterAssemblyByBasicInterface(GetType().Assembly);
            return IocRegister.GetServiceProvider(services);
        }
    }
}
