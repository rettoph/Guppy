namespace Guppy.Core.Common.Utilities
{
    public struct UnmanagedReference<TValue> : IRef<TValue>, IDisposable
    {
        private readonly int _index;

        public TValue Value
        {
            get => _values[_index];
            set => _values[_index] = value;
        }

        public UnmanagedReference()
        {
            _index = Pop();
        }

        public UnmanagedReference(TValue value)
        {
            _index = Pop();
            this.Value = value;
        }

        public void Dispose(bool disposeValue)
        {
            Push(_index);

            if (disposeValue && this.Value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void SetValue(TValue value)
        {
            this.Value = value;
        }

        public static implicit operator TValue(UnmanagedReference<TValue> value)
        {
            return value.Value;
        }

        private static readonly Stack<int> _indices = new Stack<int>();
        private static readonly List<TValue> _values = new List<TValue>() { default! };

        private static int Pop()
        {
            if (_indices.TryPop(out int index))
            {
                return index;
            }

            _values.Add(default!);
            int id = _values.Count - 1;

            return id;
        }

        private static void Push(int index)
        {
            if (_values[index] is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _values[index] = default!;
            _indices.Push(index);
        }
    }

    public struct UnmanagedReference<TNamespace, TValue> : IRef<TValue>, IDisposable
    {
        private readonly int _index;

        public TValue Value
        {
            get => _values[_index];
            set => _values[_index] = value;
        }

        public UnmanagedReference()
        {
            _index = Pop();
        }

        public UnmanagedReference(TValue value)
        {
            _index = Pop();
            this.Value = value;
        }

        public void Dispose(bool disposeValue)
        {
            Push(_index);

            if (disposeValue && this.Value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void SetValue(TValue value)
        {
            this.Value = value;
        }

        public static implicit operator TValue(UnmanagedReference<TNamespace, TValue> value)
        {
            return value.Value;
        }

        private static readonly Stack<int> _indices = new Stack<int>();
        private static readonly List<TValue> _values = new List<TValue>() { default! };

        private static int Pop()
        {
            if (_indices.TryPop(out int index))
            {
                return index;
            }

            _values.Add(default!);
            int id = _values.Count - 1;

            return id;
        }

        private static void Push(int index)
        {
            if (_values[index] is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _values[index] = default!;
            _indices.Push(index);
        }
    }
}
