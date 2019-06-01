using DapperExtensions.Mapper;
using Creekdream.Dependency;
using Creekdream.Domain.Repositories;
using Creekdream.Orm.Dapper;
using Microsoft.Extensions.DependencyInjection;
using Creekdream.Uow;
using System;
using Creekdream.Orm.Uow;

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
        public static ServicesBuilderOptions UseDapper(this ServicesBuilderOptions options, Action<UnitOfWorkOptions> uowOptions = null)
        {
            options.UseUnitOfWork(uowOptions);
            options.Services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            options.Services.AddTransient(typeof(IDatabaseProvider), typeof(DatabaseProvider));
            options.Services.RegisterAssemblyByBasicInterface(typeof(DapperServicesBuilderExtension).Assembly);
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);
            return options;
        }
    }
}

