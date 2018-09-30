using Microsoft.EntityFrameworkCore;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <inheritdoc />
    public abstract class DbContextBase : DbContext
    {
        /// <summary>
        /// Mark if DbContext has been released
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public DbContextBase(DbContextOptions options) : base(options)
        {

        }

        /// <inheritdoc />
        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }
    }
}

