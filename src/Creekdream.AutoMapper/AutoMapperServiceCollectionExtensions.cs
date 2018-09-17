using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream.AutoMapper
{
    /// <summary>
    /// AutoMapper service extensions
    /// </summary>
    public static class AutoMapperServiceCollectionExtensions
    {
        /// <summary>
        /// Add entity mapping service
        /// </summary>
        public static void AddAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> config = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            Mapper.Initialize(
                options =>
                {
                    options.ValidateInlineMaps = false;
                    config?.Invoke(options);
                });
        }
    }
}

