using System;

namespace Creekdream.System
{
    /// <inheritdoc />
    public sealed class NullDisposable : IDisposable
    {
        /// <inheritdoc />
        public static NullDisposable Instance { get; } = new NullDisposable();

        private NullDisposable()
        {

        }

        /// <inheritdoc />
        public void Dispose()
        {

        }
    }
}
