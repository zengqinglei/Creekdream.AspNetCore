namespace Creekdream.Mapping
{
    /// <summary>
    /// Defines a simple interface to map objects.
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>
        /// Converts an object to another.
        /// </summary>
        TDestination Map<TDestination>(object source);

        /// <summary>
        /// Execute a mapping from the source object to the existing destination object
        /// </summary>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
