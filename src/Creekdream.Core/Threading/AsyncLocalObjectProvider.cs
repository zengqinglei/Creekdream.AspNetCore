using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Creekdream.Threading
{
    /// <inheritdoc />
    public class AsyncLocalObjectProvider : IAsyncLocalObjectProvider
    {
        private static readonly AsyncLocal<List<object>> _asyncLocalObjects = new AsyncLocal<List<object>>();

        private List<object> GetOrInitialize()
        {
            if (_asyncLocalObjects.Value == null)
            {
                _asyncLocalObjects.Value = new List<object>();
            }
            return _asyncLocalObjects.Value.Where(value => value != null).ToList();
        }

        /// <inheritdoc />
        public T GetCurrent<T>() where T : class
        {
            var item = GetOrInitialize().SingleOrDefault(o => o.GetType() == typeof(T));
            if (item == null)
            {
                item = default(T);
            }
            return (T)item;
        }

        /// <inheritdoc />
        public void SetCurrent<T>(T value) where T : class
        {
            var item = GetOrInitialize().SingleOrDefault(o => o.GetType() == typeof(T));
            if (item != null)
            {
                _asyncLocalObjects.Value.Remove(item);
            }
            _asyncLocalObjects.Value.Add(value);
        }
    }
}
