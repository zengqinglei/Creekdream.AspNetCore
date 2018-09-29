using Creekdream.Dependency;

namespace Creekdream
{
    /// <summary>
    /// Application framework usage
    /// </summary>
    public class AppBuilderOptions
    {
        /// <summary>
        /// Ioc resolver
        /// </summary>
        public readonly IIocResolver IocResolver;

        /// <inheritdoc />
        public AppBuilderOptions(IIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }
    }
}
