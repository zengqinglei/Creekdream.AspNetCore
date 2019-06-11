using Creekdream.Dependency;
using Creekdream.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream
{
    /// <summary>
    /// Application framework initialization
    /// </summary>
    public class ServicesBuilderOptions
    {
        /// <summary>
        /// services
        /// </summary>
        public IServiceCollection Services { get; }

        /// <inheritdoc />
        public ServicesBuilderOptions(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Build and initialize
        /// </summary>
        public void Initialize()
        {
            Services.RegisterAssemblyByBasicInterface(GetType().Assembly);
        }
    }
}
