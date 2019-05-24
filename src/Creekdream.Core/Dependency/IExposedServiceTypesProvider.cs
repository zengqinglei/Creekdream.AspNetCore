using System;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Defined to expose service interfaces
    /// </summary>
    public interface IExposedServiceTypesProvider
    {
        /// <summary>
        /// Get exposed service types
        /// </summary>
        Type[] GetExposedServiceTypes(Type targetType);
    }
}
