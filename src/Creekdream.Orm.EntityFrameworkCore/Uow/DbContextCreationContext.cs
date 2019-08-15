using Creekdream.System;
using System;
using System.Data.Common;
using System.Threading;

namespace Creekdream.Orm.Uow
{
    /// <summary>
    /// DbContext creation context
    /// </summary>
    public class DbContextCreationContext
    {
        /// <summary>
        /// Current dbContext creation context
        /// </summary>
        public static DbContextCreationContext Current => _current.Value;
        private static readonly AsyncLocal<DbContextCreationContext> _current = new AsyncLocal<DbContextCreationContext>();

        /// <summary>
        /// Existing connection
        /// </summary>
        public DbConnection ExistingConnection { get; set; }

        /// <inheritdoc />
        public static IDisposable Use(DbContextCreationContext context)
        {
            var previousValue = Current;
            _current.Value = context;
            return new DisposeAction(() => _current.Value = previousValue);
        }
    }
}
