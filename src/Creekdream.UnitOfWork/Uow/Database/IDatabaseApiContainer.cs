using Creekdream.Dependency;
using System;

namespace Creekdream.Uow
{
    /// <summary>
    /// Database api container
    /// </summary>
    public interface IDatabaseApiContainer
    {
        /// <summary>
        /// Get or add database api
        /// </summary>
        IDatabaseApi GetOrAddDatabaseApi(Func<IDatabaseApi> factory);
    }
}
