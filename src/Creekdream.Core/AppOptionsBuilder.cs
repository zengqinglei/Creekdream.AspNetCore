using Creekdream.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream
{
    /// <summary>
    /// Application framework initialization
    /// </summary>
    public class AppOptionsBuilder
    {
        /// <summary>
        /// Ioc register
        /// </summary>
        public IocRegisterBase IocRegister { get; set; }

        /// <summary>
        /// Build and initialize
        /// </summary>
        public IServiceProvider Build(IServiceCollection services)
        {
            return IocRegister.GetServiceProvider(services);
        }
    }
}
