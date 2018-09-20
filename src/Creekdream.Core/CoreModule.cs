using Creekdream.Dependency;

namespace Creekdream
{
    /// <summary>
    /// Base class of module
    /// </summary>
    public abstract class CoreModule
    {
        /// <summary>
        /// Loading module, generally used for injection dependency
        /// </summary>
        public virtual void ConfigureServices(IocRegisterBase iocRegister)
        {

        }

        /// <summary>
        /// Loading module, generally used to enable services
        /// </summary>
        public virtual void Configure(IIocResolver iocResolver)
        {

        }
    }
}
