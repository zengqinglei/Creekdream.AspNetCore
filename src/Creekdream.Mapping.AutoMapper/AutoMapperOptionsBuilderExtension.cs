using AutoMapper;
using System;

namespace Creekdream.Mapping
{
    /// <summary>
    /// AutoMapper specific extension methods for <see cref="AppOptionsBuilder" />.
    /// </summary>
    public static class AutoMapperServiceCollectionExtensions
    {
        /// <summary>
        /// Use AutoMapper
        /// </summary>
        public static AppOptionsBuilder UseAutoMapper(this AppOptionsBuilder builder, Action<IMapperConfigurationExpression> config = null)
        {
            Mapper.Initialize(
                options =>
                {
                    options.ValidateInlineMaps = false;
                    config?.Invoke(options);
                });
            return builder;
        }

        /// <summary>
        /// Map out new target objects
        /// </summary>
        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Map attribute content to target object
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }
    }
}

