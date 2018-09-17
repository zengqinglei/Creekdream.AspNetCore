using System.Linq;
using System.Reflection;
using Creekdream.Dependency;

namespace Creekdream.UnitOfWork
{
    /// <inheritdoc />
    public class UnitOfWorkCoreOptionsBuilder
    {
        /// <summary>
        /// Uow options
        /// </summary>
        public UnitOfWorkOptions UowOptions { get; set; }

        /// <inheritdoc />
        public UnitOfWorkCoreOptionsBuilder()
        {
            UowOptions = new UnitOfWorkOptions();
        }

        /// <summary>
        /// Build a unit of work module
        /// </summary>
        public void Build(IocRegisterBase iocRegister)
        {
            iocRegister.Register<UnitOfWorkOptions>();
            iocRegister.RegisterAssemblyByBasicInterface(typeof(UnitOfWorkCoreOptionsBuilder).Assembly);
            iocRegister.RegisterInterceptor<UnitOfWorkInterceptor>(
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
                    return false;
                });
        }
    }
}

