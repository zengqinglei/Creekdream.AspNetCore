using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream.Orm
{
    /// <summary>
    /// Dapper service extension
    /// </summary>
    public static class DapperkServiceCollectionExtensions
    {
        /// <summary>
        /// Add Dapper's DbConnect connection
        /// </summary>
        public static void AddDbConnection(this IServiceCollection services, Action<DapperOptionsBuilder> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var dapperOptions = new DapperOptionsBuilder();
            options.Invoke(dapperOptions);

            services.AddSingleton(dapperOptions);
        }
    }
}

