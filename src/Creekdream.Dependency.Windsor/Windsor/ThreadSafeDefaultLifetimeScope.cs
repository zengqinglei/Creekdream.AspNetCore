using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle.Scoped;
using System;

namespace Creekdream.Dependency.Windsor
{
    /// <summary>
    /// Default lifetimescope within the scope of a secure thread
    /// </summary>
    public class ThreadSafeDefaultLifetimeScope : ILifetimeScope
    {
        private static readonly Action<Burden> emptyOnAfterCreated = delegate { };
        private readonly object @lock = new object();
        private readonly Action<Burden> onAfterCreated;
        private IScopeCache scopeCache;

        /// <inheritdoc />
        public ThreadSafeDefaultLifetimeScope(IScopeCache scopeCache = null, Action<Burden> onAfterCreated = null)
        {
            this.scopeCache = scopeCache ?? new ScopeCache();
            this.onAfterCreated = onAfterCreated ?? emptyOnAfterCreated;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            lock (@lock)
            {
                if (scopeCache == null)
                {
                    return;
                }
                if (scopeCache is IDisposable disposableCache)
                {
                    disposableCache.Dispose();
                }
                scopeCache = null;
            }
        }

        /// <inheritdoc />
        public Burden GetCachedInstance(ComponentModel model, ScopedInstanceActivationCallback createInstance)
        {
            lock (@lock)
            {
                var burden = scopeCache[model];
                if (burden == null)
                {
                    scopeCache[model] = burden = createInstance(onAfterCreated);
                }
                return burden;
            }
        }
    }
}
