using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;
using System.Threading;

namespace Creekdream.Dependency.Windsor
{
    /// <summary>
    /// Implement a lifetimescope like Autofac
    /// </summary>
    public class LocalLifetimeScopeAccessor : IScopeAccessor
    {
        private static readonly AsyncLocal<ThreadSafeDefaultLifetimeScope> AsyncLocalScope = new AsyncLocal<ThreadSafeDefaultLifetimeScope>();

        /// <inheritdoc />
        public ILifetimeScope GetScope(CreationContext context)
        {
            if (AsyncLocalScope.Value == null)
            {
                AsyncLocalScope.Value = new ThreadSafeDefaultLifetimeScope();
            }
            return AsyncLocalScope.Value;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            AsyncLocalScope.Value.Dispose();
            AsyncLocalScope.Value = null;
        }
    }
}
