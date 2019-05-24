using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Dependency attribute
    /// </summary>
    public class DependencyAttribute : Attribute
    {
        /// <summary>
        /// Lifetime
        /// </summary>
        public virtual ServiceLifetime? Lifetime { get; set; }

        /// <summary>
        /// Try register
        /// </summary>
        public virtual bool TryRegister { get; set; }

        /// <summary>
        /// Replace services
        /// </summary>
        public virtual bool ReplaceServices { get; set; }

        /// <inheritdoc />
        public DependencyAttribute()
        {

        }

        /// <inheritdoc />
        public DependencyAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}
