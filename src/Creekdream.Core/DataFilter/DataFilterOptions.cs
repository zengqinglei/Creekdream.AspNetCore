using System;
using System.Collections.Generic;

namespace Creekdream.DataFilter
{
    /// <summary>
    /// Data filter options
    /// </summary>
    public class DataFilterOptions
    {
        /// <summary>
        /// Default states
        /// </summary>
        public Dictionary<Type, DataFilterState> DefaultStates { get; }

        /// <inheritdoc />
        public DataFilterOptions()
        {
            DefaultStates = new Dictionary<Type, DataFilterState>();
        }
    }
}
