using DapperExtensions.Sql;
using MySql.Data.MySqlClient;

namespace Creekdream.Orm
{
    /// <summary>
    /// MySql specific extension methods for <see cref="DapperOptionsBuilder" />.
    /// </summary>
    public static class MySqlServicesBuilderExtensions
    {
        /// <summary>
        /// Use Mysql
        /// </summary>
        public static void UseMySql(this DapperOptionsBuilder builder, string connectionString)
        {
            builder.GetDbConnection = () =>
            {
                return new MySqlConnection(connectionString);
            };
            builder.SqlDialect = new MySqlDialect();
        }
    }
}
