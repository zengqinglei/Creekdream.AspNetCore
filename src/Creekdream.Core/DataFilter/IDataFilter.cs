using System;

namespace Creekdream.DataFilter
{
    /// <summary>
    /// Data filter
    /// </summary>
    public interface IDataFilter
    {
        /// <summary>
        /// Enable filter
        /// </summary>
        IDisposable Enable<TFilter>()
            where TFilter : class;

        /// <summary>
        /// Disable filter
        /// </summary>
        IDisposable Disable<TFilter>()
            where TFilter : class;

        /// <summary>
        /// Is enable
        /// </summary>
        bool IsEnabled<TFilter>()
            where TFilter : class;
    }

    /// <summary>
    /// Data filter
    /// </summary>
    public interface IDataFilter<TFilter>
        where TFilter : class
    {
        /// <summary>
        /// Enable filter
        /// </summary>
        IDisposable Enable();

        /// <summary>
        /// Disable filter
        /// </summary>
        IDisposable Disable();

        /// <summary>
        /// Is enable
        /// </summary>
        bool IsEnabled { get; }
    }
}
