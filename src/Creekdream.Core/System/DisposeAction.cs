using System;

namespace Creekdream.System
{
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
