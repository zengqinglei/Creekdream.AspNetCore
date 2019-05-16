using System;

namespace Creekdream
{
    /// <summary>
    /// Application framework usage
    /// </summary>
    public class AppBuilderOptions
    {
        /// <summary>
        /// Service provider
        /// </summary>
        public readonly IServiceProvider ServiceProvider;

        /// <inheritdoc />
        public AppBuilderOptions(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
