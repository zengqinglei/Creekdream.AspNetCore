namespace Creekdream.Dependency
{
    /// <summary>
    /// Dependent service lifecycle approach
    /// </summary>
    public enum DependencyLifeStyle
    {
        /// <summary>
        /// Singleton, follow the application life cycle
        /// </summary>
        Singleton = 0,

        /// <summary>
        /// Scoped, follow the life cycle of Http per request
        /// </summary>
        Scoped = 1,

        /// <summary>
        /// Transient, an instance of the scope
        /// </summary>
        Transient = 2
    }
}

