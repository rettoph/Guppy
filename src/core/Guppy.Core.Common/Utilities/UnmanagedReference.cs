namespace Guppy.Core.Common.Utilities
{
    /// <summary>
    /// Offers an unmanaged struct that can be used as a reference to a managed object
    /// This is done with static magic and technically not safe, as the managed object
    /// can be disposed or removed at any time. Use with care...
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public readonly struct UnmanagedReference<TValue> : IRef<TValue>, IDisposable
    {
        private readonly int _index;

        public readonly TValue Value
        {
            get => _values[this._index];
            set => _values[this._index] = value;
        }

        public readonly Type Type => typeof(TValue);

        readonly object? IRef.Value => this.Value;

        public UnmanagedReference()
        {
            this._index = Pop();
        }

        public UnmanagedReference(TValue value)
        {
            this._index = Pop();
            this.Value = value;
        }

        public readonly void Dispose(bool disposeValue)
        {
            Push(this._index);

            if (disposeValue && this.Value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public readonly void Dispose() => this.Dispose(true);

        public void SetValue(TValue value) => this.Value = value;

        public static implicit operator TValue(UnmanagedReference<TValue> value)
        {
            return value.Value;
        }

        private static readonly Stack<int> _indices = new();
        private static readonly List<TValue> _values = [default!];

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

        public override readonly bool Equals(object? obj) => obj is UnmanagedReference<TValue> reference &&
                   this._index == reference._index;

        public override readonly int GetHashCode() => HashCode.Combine(this._index);

        public static bool operator ==(UnmanagedReference<TValue> left, UnmanagedReference<TValue> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UnmanagedReference<TValue> left, UnmanagedReference<TValue> right)
        {
            return !(left == right);
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
    public readonly struct UnmanagedReference<TNamespace, TValue> : IRef<TValue>, IDisposable
    {
        private readonly int _index;

        public readonly TValue Value
        {
            get => _values[this._index];
            set => _values[this._index] = value;
        }

        public readonly Type Type => typeof(TValue);

        readonly object? IRef.Value => this.Value;

        public UnmanagedReference()
        {
            this._index = Pop();
        }

        public UnmanagedReference(TValue value)
        {
            this._index = Pop();
            this.Value = value;
        }

        public readonly void Dispose(bool disposeValue)
        {
            Push(this._index);

            if (disposeValue && this.Value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public readonly void Dispose() => this.Dispose(true);

        public void SetValue(TValue value) => this.Value = value;

        public static implicit operator TValue(UnmanagedReference<TNamespace, TValue> value)
        {
            return value.Value;
        }

        private static readonly Stack<int> _indices = new();
        private static readonly List<TValue> _values = [default!];

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

        public override readonly bool Equals(object? obj) => obj is UnmanagedReference<TNamespace, TValue> reference &&
                   this._index == reference._index;

        public override readonly int GetHashCode() => HashCode.Combine(this._index);

        public static bool operator ==(UnmanagedReference<TNamespace, TValue> left, UnmanagedReference<TNamespace, TValue> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UnmanagedReference<TNamespace, TValue> left, UnmanagedReference<TNamespace, TValue> right)
        {
            return !(left == right);
        }
    }
}