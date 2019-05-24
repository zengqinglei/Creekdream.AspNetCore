using Creekdream.System.Collections;
using System;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Service registration context
    /// </summary>
    public interface IOnServiceRegistredContext
    {
        /// <summary>
        /// Interceptors
        /// </summary>
        ITypeList<InterceptorBase> Interceptors { get; }

        /// <summary>
        /// Implementation type
        /// </summary>
        Type ImplementationType { get; }
    }
}
