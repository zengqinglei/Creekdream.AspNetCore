using Creekdream.Dependency;
using Creekdream.Domain.Repositories;
using Creekdream.Orm.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        public static ServicesBuilderOptions UseEfCore(this ServicesBuilderOptions builder)
        {
            builder.Services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            builder.Services.RegisterAssemblyByBasicInterface(typeof(EfCoreServicesBuilderExtension).Assembly);
            return builder;
        }
    }
}

