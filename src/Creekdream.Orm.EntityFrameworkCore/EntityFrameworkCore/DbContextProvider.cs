using Microsoft.EntityFrameworkCore;
using Creekdream.Dependency;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <inheritdoc />
    public class DbContextProvider : IDbContextProvider
    {
        private class LocalDbContextWapper
        {
            public DbContext DbContext { get; set; }

            public IDbContextTransaction DbContextTransaction { get; set; }
        }

        private static readonly AsyncLocal<LocalDbContextWapper> AsyncLocalDbContext = new AsyncLocal<LocalDbContextWapper>();
        
        private readonly IIocResolver _iocResolver;

        /// <inheritdoc />
        public DbContextProvider(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        /// <inheritdoc />
        public DbContext GetDbContext()
        {
            if (AsyncLocalDbContext.Value == null)
            {
                AsyncLocalDbContext.Value = new LocalDbContextWapper()
                {
                    DbContext = _iocResolver.Resolve<DbContext>()
                };
            }
            return AsyncLocalDbContext.Value.DbContext;
        }

        /// <inheritdoc />
        public IDbContextTransaction DbContextTransaction
        {
            get
            {
                return AsyncLocalDbContext.Value.DbContextTransaction;
            }
            set
            {
                AsyncLocalDbContext.Value.DbContextTransaction = value;
            }
        }
    }
}

