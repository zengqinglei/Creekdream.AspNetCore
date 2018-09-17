using Microsoft.EntityFrameworkCore;

namespace Creekdream.Orm.EntityFrameworkCore
{
    /// <inheritdoc />
    public abstract class DbContextBase : DbContext
    {
        /// <inheritdoc />
        public DbContextBase(DbContextOptions options) : base(options)
        {

        }
    }
}

