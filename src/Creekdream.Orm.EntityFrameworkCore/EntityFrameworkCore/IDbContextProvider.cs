using Microsoft.EntityFrameworkCore;
using Creekdream.Dependency;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <summary>
    /// DbContext provider
    /// </summary>
    public interface IDbContextProvider : ITransientDependency
    {
        /// <summary>
        /// Get data context operation object
        /// </summary>
        DbContextBase GetDbContext();

        /// <summary>
        /// DbContext transaction
        /// </summary>
        IDbContextTransaction DbContextTransaction { get; set; }
    }
}

