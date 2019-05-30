using Creekdream.Dependency;
using Creekdream.Domain.Repositories;
using Creekdream.Orm.EntityFrameworkCore;
using Creekdream.Orm.Uow;
using Creekdream.Uow;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream.Orm
{
    /// <summary>
    /// EfCore specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class EfCoreServicesBuilderExtension
    {
        /// <summary>
        /// Use EfCore Module
        /// </summary>
        public static ServicesBuilderOptions UseEfCore(this ServicesBuilderOptions options, Action<UnitOfWorkOptions> uowOptions = null)
        {
            options.UseUnitOfWork(uowOptions);
            options.Services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            options.Services.AddTransient(typeof(IDbContextProvider), typeof(DbContextProvider));
            options.Services.RegisterAssemblyByBasicInterface(typeof(EfCoreServicesBuilderExtension).Assembly);
            return options;
        }
    }
}

