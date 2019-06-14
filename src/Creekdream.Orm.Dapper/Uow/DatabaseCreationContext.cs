using System;
using System.Data.Common;
using System.Threading;

namespace Creekdream.Orm.Uow
{
    /// <summary>
    /// Database creation context
    /// </summary>
    public class DatabaseCreationContext
    {
        /// <summary>
        /// Current dbContext creation context
        /// </summary>
        public static DatabaseCreationContext Current => _current.Value;
        private static readonly AsyncLocal<DatabaseCreationContext> _current = new AsyncLocal<DatabaseCreationContext>();

        /// <summary>
        /// Existing connection
        /// </summary>
        public DbConnection ExistingConnection { get; set; }

        /// <inheritdoc />
        public static IDisposable Use(DatabaseCreationContext context)
        {
            var previousValue = Current;
            _current.Value = context;
            return new DisposeAction(() => _current.Value = previousValue);
        }
    }

    /// <summary>
    /// Dispose action
    /// </summary>
    public class DisposeAction : IDisposable
    {
        private readonly Action _action;

        /// <summary>
        /// Creates a new <see cref="DisposeAction"/> object.
        /// </summary>
        /// <param name="action">Action to be executed when this object is disposed.</param>
        public DisposeAction(Action action)
        {
            _action = action;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _action();
        }
    }
}
