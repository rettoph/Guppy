namespace Guppy.Resources.Utilities
{
    internal static class StaticCollection<T>
    {
        private static readonly HashSet<T> _owners = new HashSet<T>();

        public static event EventHandler<T>? OnAdded;
        public static event EventHandler<T>? OnRemoved;

        public static void Add(T owner)
        {
            if (_owners.Add(owner) == false)
            {
                return;
            }

            OnAdded?.Invoke(null, owner);
        }

        public static void Remove(T owner, bool dispose)
        {
            if (_owners.Remove(owner) == false)
            {
                return;
            }

            if (dispose && owner is IDisposable disposable)
            {
                disposable.Dispose();
            }

            OnRemoved?.Invoke(null, owner);
        }

        public static IEnumerable<T> GetAll()
        {
            return _owners;
        }

        public static void Clear()
        {
            // TODO: This should clear and dispose all resources
            foreach (IDisposable disposable in _owners.OfType<IDisposable>().ToList())
            {
                disposable.Dispose();
            }

            _owners.Clear();
        }
    }

    internal static class StaticValueCollection<T, TValue>
        where T : notnull
    {
        private static readonly Stack<int> _indices = new Stack<int>();
        private static readonly List<TValue> _values = new List<TValue>();

        public static TValue Get(int index)
        {
            return _values[index];
        }

        public static TValue Set(int index, TValue value)
        {
            return _values[index] = value;
        }

        public static int Pop()
        {
            if (_indices.TryPop(out int index))
            {
                return index;
            }

            _values.Add(default!);
            int id = _values.Count - 1;

            return id;
        }

        public static void Push(int index)
        {
            if (_values[index] is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _values[index] = default!;
            _indices.Push(index);
        }

        public static void Clear()
        {
            foreach (IDisposable disposable in _values.OfType<IDisposable>())
            {
                disposable.Dispose();
            }

            _values.Clear();
            _indices.Clear();
        }
    }
}
