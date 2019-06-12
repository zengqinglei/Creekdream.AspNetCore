using Creekdream.Uow;
using DapperExtensions;

namespace Creekdream.Orm.Uow
{
    /// <inheritdoc />
    public class DatabaseApi : IDatabaseApi
    {
        /// <inheritdoc />
        public IDatabase Database { get; }

        /// <inheritdoc />
        public DatabaseApi(IDatabase database)
        {
            Database = database;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
