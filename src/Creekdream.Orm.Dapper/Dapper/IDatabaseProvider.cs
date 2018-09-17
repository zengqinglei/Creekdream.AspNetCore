using System;
using System.Data.Common;
using Creekdream.Dependency;
using DapperExtensions;

namespace Creekdream.Orm.Dapper
{
    /// <summary>
    /// Database provider
    /// </summary>
    public interface IDatabaseProvider : ITransientDependency, IDisposable
    {
        /// <summary>
        /// Get a database
        /// </summary>
        IDatabase GetDatabase();

        /// <summary>
        /// Database transaction
        /// </summary>
        DbTransaction DbTransaction { get; set; }
    }
}

