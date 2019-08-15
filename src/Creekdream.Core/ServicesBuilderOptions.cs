using Creekdream.DataFilter;
using Creekdream.Dependency;
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
            Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
            Services.RegisterAssemblyByBasicInterface(GetType().Assembly);
        }
    }
}
