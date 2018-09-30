using Creekdream.Dependency;
using Creekdream.Threading;
using Microsoft.EntityFrameworkCore.Storage;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <inheritdoc />
    public class DbContextProvider : IDbContextProvider
    {
        private class LocalDbContextWapper
        {
            public DbContextBase DbContext { get; set; }

            public IDbContextTransaction DbContextTransaction { get; set; }
        }

        private readonly IAsyncLocalObjectProvider _asyncLocalObjectProvider;
        private readonly IIocResolver _iocResolver;

        /// <inheritdoc />
        public DbContextProvider(IAsyncLocalObjectProvider asyncLocalObjectProvider, IIocResolver iocResolver)
        {
            _asyncLocalObjectProvider = asyncLocalObjectProvider;
            _iocResolver = iocResolver;
        }

        /// <inheritdoc />
        public DbContextBase GetDbContext()
        {
            var localDbContext = _asyncLocalObjectProvider.GetCurrent<LocalDbContextWapper>();
            if (localDbContext == null || localDbContext.DbContext == null || localDbContext.DbContext.IsDisposed)
            {
                localDbContext = new LocalDbContextWapper()
                {
                    DbContext = _iocResolver.Resolve<DbContextBase>()
                };
                _asyncLocalObjectProvider.SetCurrent(localDbContext);
            }
            return localDbContext.DbContext;
        }

        /// <inheritdoc />
        public IDbContextTransaction DbContextTransaction
        {
            get => _asyncLocalObjectProvider.GetCurrent<LocalDbContextWapper>()?.DbContextTransaction;
            set => _asyncLocalObjectProvider.GetCurrent<LocalDbContextWapper>().DbContextTransaction = value;
        }
    }
}

