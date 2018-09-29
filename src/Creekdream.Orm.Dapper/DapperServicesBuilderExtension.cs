using DapperExtensions.Mapper;
using Creekdream.Dependency;
using Creekdream.Domain.Repositories;
using Creekdream.Orm.Dapper;

namespace Creekdream.Orm
{
    /// <summary>
    /// Dapper specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class DapperServicesBuilderExtension
    {
        /// <summary>
        /// Use Dapper as the Orm framework
        /// </summary>
        public static ServicesBuilderOptions UseDapper(this ServicesBuilderOptions builder)
        {
            builder.IocRegister.Register(
                typeof(IRepository<,>), typeof(RepositoryBase<,>),
                lifeStyle: DependencyLifeStyle.Transient);
            builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(DapperServicesBuilderExtension).Assembly);
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);
            return builder;
        }
    }
}

