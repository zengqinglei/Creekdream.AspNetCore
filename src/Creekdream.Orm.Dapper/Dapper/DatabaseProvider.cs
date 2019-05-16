using Creekdream.Dependency;
using Creekdream.Orm.Uow;
using Creekdream.Uow;
using DapperExtensions;
using System.Data.Common;

namespace Creekdream.Orm.Dapper
{
    /// <inheritdoc />
    public class DatabaseProvider : IDatabaseProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        /// <inheritdoc />
        public DatabaseProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        /// <inheritdoc />
        public IDatabase GetDatabase()
        {
            return _currentUnitOfWorkProvider.Get().GetDatabase();
        }

        /// <inheritdoc />
        public DbTransaction GetDbTransaction()
        {
            return _currentUnitOfWorkProvider.Get().GetDbTransaction();
        }
    }
}

