using DapperExtensions.Mapper;
using Creekdream.Dependency;
using Creekdream.Domain.Repositories;
using Creekdream.Orm.Dapper;

namespace Creekdream.Orm
{
    /// <summary>
    /// Dapper specific extension methods for <see cref="AppOptionsBuilder" />.
    /// </summary>
    public static class DapperOptionsBuilderExtension
    {
        /// <summary>
        /// Use Dapper as the Orm framework
        /// </summary>
        public static AppOptionsBuilder UseDapper(this AppOptionsBuilder builder)
        {
            builder.IocRegister.Register(
                typeof(IRepository<,>), typeof(RepositoryBase<,>),
                lifeStyle: DependencyLifeStyle.Transient);
            builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(DapperOptionsBuilderExtension).Assembly);
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);
            return builder;
        }
    }
}

