using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Used to convert registration abstract classes
    /// </summary>
    public abstract class ConventionalRegistrarBase : IConventionalRegistrar
    {
        /// <inheritdoc />
        public virtual void AddAssembly(IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(
                    type => type != null &&
                            type.IsClass &&
                            !type.IsAbstract &&
                            !type.IsGenericType
                ).ToArray();

            AddTypes(services, types);
        }

        /// <inheritdoc />
        public virtual void AddTypes(IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                AddType(services, type);
            }
        }

        /// <inheritdoc />
        public abstract void AddType(IServiceCollection services, Type type);
    }
}
