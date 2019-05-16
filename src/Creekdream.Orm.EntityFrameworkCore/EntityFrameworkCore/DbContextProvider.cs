using Creekdream.Dependency;
using Creekdream.Orm.Uow;
using Creekdream.Uow;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <inheritdoc />
    public class DbContextProvider : IDbContextProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        /// <inheritdoc />
        public DbContextProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        /// <inheritdoc />
        public DbContextBase GetDbContext()
        {
            return _currentUnitOfWorkProvider.Get().GetDbContext();
        }
    }
}

