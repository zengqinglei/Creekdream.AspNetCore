using Creekdream.Dependency;

namespace Creekdream.Threading
{
    /// <summary>
    /// Define an AsyncLocal based on the thread storage object
    /// </summary>
    public interface IAsyncLocalObjectProvider : ISingletonDependency
    {
        /// <summary>
        /// Get the T object in the current thread
        /// </summary>
        T GetCurrent<T>() where T : class;

        /// <summary>
        /// Set the T type object to the current thread
        /// </summary>
        void SetCurrent<T>(T value) where T : class;
    }
}
