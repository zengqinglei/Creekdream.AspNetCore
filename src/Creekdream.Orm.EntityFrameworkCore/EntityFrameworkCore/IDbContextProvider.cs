using Microsoft.EntityFrameworkCore;
using Creekdream.Dependency;
using Microsoft.EntityFrameworkCore.Storage;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <summary>
    /// DbContext provider
    /// </summary>
    public interface IDbContextProvider : ISingletonDependency
    {
        /// <summary>
        /// Get data context operation object
        /// </summary>
        DbContext GetDbContext();

        /// <summary>
        /// DbContext transaction
        /// </summary>
        IDbContextTransaction DbContextTransaction { get; set; }
    }
}

