using DapperExtensions.Sql;
using Oracle.ManagedDataAccess.Client;

namespace Creekdream.Orm
{
    /// <summary>
    /// Oracle specific extension methods for <see cref="DapperOptionsBuilder" />.
    /// </summary>
    public static class OracleServicesBuilderExtensions
    {
        /// <summary>
        /// Use Oracle
        /// </summary>
        public static void UseOracle(this DapperOptionsBuilder builder, string connectionString)
        {
            builder.GetDbConnection = () =>
            {
                return new OracleConnection(connectionString);
            };
            builder.SqlDialect = new OracleDialect();
        }
    }
}
