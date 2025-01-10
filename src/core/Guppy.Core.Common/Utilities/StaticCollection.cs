namespace Guppy.Core.Common.Utilities
{
    public static class StaticCollection<T>
    {
        private static readonly HashSet<T> _items = [];

        public static event EventHandler<T>? OnAdded;
        public static event EventHandler<T>? OnRemoved;

        public static void Add(T owner)
        {
            if (_items.Add(owner) == false)
            {
                return;
            }

            OnAdded?.Invoke(null, owner);
        }

        public static void Remove(T item, bool dispose)
        {
            if (_items.Remove(item) == false)
            {
                return;
            }

            if (dispose && item is IDisposable disposable)
            {
                disposable.Dispose();
            }

            OnRemoved?.Invoke(null, item);
        }

        public static IEnumerable<T> GetAll() => _items;

        public static void Clear(bool dispose)
        {
            while (_items.Count > 0)
            {
                Remove(_items.First(), dispose);
            }
        }
    }
}