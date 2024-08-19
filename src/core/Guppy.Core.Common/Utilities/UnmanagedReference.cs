namespace Guppy.Core.Common.Utilities
{
    /// <summary>
    /// Offers an unmanaged struct that can be used as a reference to a managed object
    /// This is done with static magic and technically not safe, as the managed object
    /// can be disposed or removed at any time. Use with care...
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public struct UnmanagedReference<TValue> : IRef<TValue>, IDisposable
    {
        private readonly int _index;

        public TValue Value
        {
            get => _values[_index];
            set => _values[_index] = value;
        }

        public Type Type => typeof(TValue);
        object? IRef.Value => this.Value;

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
            _values[index] = default!;
            _indices.Push(index);
        }

        public override bool Equals(object? obj)
        {
            return obj is UnmanagedReference<TValue> reference &&
                   _index == reference._index;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_index);
        }
    }

    /// <summary>
    /// Offers an unmanaged struct that can be used as a reference to a managed object
    /// This is done with static magic and technically not safe, as the managed object
    /// can be disposed or removed at any time. Use with care.
    /// 
    /// The Namespace simply offers an extra layer of distinction between the backing static dictionary.
    /// </summary>
    /// <typeparam name="TNamespace"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public struct UnmanagedReference<TNamespace, TValue> : IRef<TValue>, IDisposable
    {
        private readonly int _index;

        public TValue Value
        {
            get => _values[_index];
            set => _values[_index] = value;
        }

        public Type Type => typeof(TValue);
        object? IRef.Value => this.Value;

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
            _values[index] = default!;
            _indices.Push(index);
        }

        public override bool Equals(object? obj)
        {
            return obj is UnmanagedReference<TNamespace, TValue> reference &&
                   _index == reference._index;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_index);
        }
    }
}
