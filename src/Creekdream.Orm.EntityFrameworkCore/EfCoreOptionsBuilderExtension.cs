using Creekdream.Dependency;
using Creekdream.Domain.Repositories;
using Creekdream.Orm.EntityFrameworkCore;

namespace Creekdream.Orm
{
    /// <summary>
    /// EfCore specific extension methods for <see cref="AppOptionsBuilder" />.
    /// </summary>
    public static class EfCoreOptionsBuilderExtension
    {
        /// <summary>
        /// Use EfCore Module
        /// </summary>
        public static AppOptionsBuilder UseEfCore(this AppOptionsBuilder builder)
        {
            builder.IocRegister.Register(
                typeof(IRepository<,>), typeof(RepositoryBase<,>),
                lifeStyle: DependencyLifeStyle.Transient);
            builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(EfCoreOptionsBuilderExtension).Assembly);
            return builder;
        }
    }
}

