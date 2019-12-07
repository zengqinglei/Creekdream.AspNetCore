using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream.AspNetCore
{
    /// <summary>
    /// Creekdream application service injection
    /// </summary>
    public static class CreekdreamServiceCollectionExtensions
    {
        /// <summary>
        /// Add application framework service
        /// </summary>
        public static IServiceCollection AddCreekdream(this IServiceCollection services, Action<ServicesBuilderOptions> options = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var builder = new ServicesBuilderOptions(services);
            options?.Invoke(builder);
            builder.Initialize();

            return services;
        }
    }
}

