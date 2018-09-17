using System;

namespace Creekdream.Dependency
{
    /// <summary>
    /// Define a resolve interface
    /// </summary>
    public interface IIocResolver : IDisposable
    {
        /// <summary>
        /// Get injected generic instance
        /// </summary>
        T Resolve<T>();

        /// <summary>
        /// Get the injected generic instance and pass in the constructor
        /// </summary>
        T Resolve<T>(object argumentsAsAnonymousType);

        /// <summary>
        /// Get an instance of the specified type
        /// </summary>
        object Resolve(Type serviceType);

        /// <summary>
        /// Get an instance of the specified type and pass parameters to the constructor
        /// </summary>
        object Resolve(Type serviceType, object argumentsAsAnonymousType);

        /// <summary>
        /// Is the service type registered?
        /// </summary>
        bool IsRegistered(Type serviceType);

        /// <summary>
        /// Is the service generic registered?
        /// </summary>
        bool IsRegistered<TService>();

        /// <summary>
        /// Release the parsed object
        /// </summary>
        void Release(object obj);
    }
}

