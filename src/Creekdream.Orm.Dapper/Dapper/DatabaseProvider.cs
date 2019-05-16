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
        /// <inheritdoc />
        public DbTransaction DbTransaction { get; set; }

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
    }
}

