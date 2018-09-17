using Microsoft.EntityFrameworkCore;
using System;
using Creekdream.Dependency;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <summary>
    /// DbContext provider
    /// </summary>
    public interface IDbContextProvider : ITransientDependency, IDisposable
    {
        /// <summary>
        /// Get data context operation object
        /// </summary>
        DbContext GetDbContext();
    }
}

