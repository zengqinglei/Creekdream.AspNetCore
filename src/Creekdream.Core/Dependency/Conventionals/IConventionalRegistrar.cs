using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Defined for conversion registration interface
    /// </summary>
    public interface IConventionalRegistrar
    {
        /// <summary>
        /// Add assembly conversions
        /// </summary>
        void AddAssembly(IServiceCollection services, Assembly assembly);

        /// <summary>
        /// Add collection type conversions
        /// </summary>
        void AddTypes(IServiceCollection services, params Type[] types);

        /// <summary>
        /// Add type conversion
        /// </summary>
        void AddType(IServiceCollection services, Type type);
    }
}
