using Creekdream.Dependency;
using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Creekdream.System;

namespace Creekdream.DataFilter
{
    /// <inheritdoc />
    public class DataFilter : IDataFilter, ISingletonDependency
    {
        private readonly ConcurrentDictionary<Type, object> _filters;
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public DataFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _filters = new ConcurrentDictionary<Type, object>();
        }

        /// <inheritdoc />
        public IDisposable Enable<TFilter>()
            where TFilter : class
        {
            return GetFilter<TFilter>().Enable();
        }

        /// <inheritdoc />
        public IDisposable Disable<TFilter>()
            where TFilter : class
        {
            return GetFilter<TFilter>().Disable();
        }

        /// <inheritdoc />
        public bool IsEnabled<TFilter>()
            where TFilter : class
        {
            return GetFilter<TFilter>().IsEnabled;
        }

        private IDataFilter<TFilter> GetFilter<TFilter>()
            where TFilter : class
        {
            if (!_filters.TryGetValue(typeof(TFilter), out object obj))
            {
                obj = _serviceProvider.GetRequiredService<IDataFilter<TFilter>>();
            }
            return obj as IDataFilter<TFilter>;
        }
    }

    /// <inheritdoc />
    public class DataFilter<TFilter> : IDataFilter<TFilter>
        where TFilter : class
    {
        /// <inheritdoc />
        public bool IsEnabled
        {
            get
            {
                EnsureInitialized();
                return _filter.Value.IsEnabled;
            }
        }

        private readonly DataFilterOptions _options;

        private readonly AsyncLocal<DataFilterState> _filter;

        /// <inheritdoc />
        public DataFilter(DataFilterOptions options)
        {
            _options = options;
            _filter = new AsyncLocal<DataFilterState>();
        }

        /// <inheritdoc />
        public IDisposable Enable()
        {
            if (IsEnabled)
            {
                return NullDisposable.Instance;
            }

            _filter.Value.IsEnabled = true;

            return new DisposeAction(() => Disable());
        }

        /// <inheritdoc />
        public IDisposable Disable()
        {
            if (!IsEnabled)
            {
                return NullDisposable.Instance;
            }

            _filter.Value.IsEnabled = false;

            return new DisposeAction(() => Enable());
        }

        private void EnsureInitialized()
        {
            if (_filter.Value != null)
            {
                return;
            }

            _options.DefaultStates.TryGetValue(typeof(TFilter), out DataFilterState dataFilter);
            _filter.Value = dataFilter?.Clone() ?? new DataFilterState(true);
        }
    }
}
