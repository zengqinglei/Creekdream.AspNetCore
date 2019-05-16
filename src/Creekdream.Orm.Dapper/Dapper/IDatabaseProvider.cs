using DapperExtensions;
using System.Data.Common;

namespace Creekdream.Orm.Dapper
{
    /// <summary>
    /// Database provider
    /// </summary>
    public interface IDatabaseProvider
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

