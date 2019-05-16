using DapperExtensions.Mapper;
using Creekdream.Dependency;
using Creekdream.Domain.Repositories;
using Creekdream.Orm.Dapper;
using Microsoft.Extensions.DependencyInjection;

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
            builder.Services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            builder.Services.RegisterAssemblyByBasicInterface(typeof(DapperServicesBuilderExtension).Assembly);
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);
            return builder;
        }
    }
}

