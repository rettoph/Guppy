namespace Guppy.Core.Common.Utilities
{
    public struct StaticValue<TNamespace, TValue> : IRef<TValue>, IDisposable
    {
        private readonly int _index;

        public TValue Value
        {
            get => _values[_index];
            set => _values[_index] = value;
        }

        public StaticValue()
        {
            _index = Pop();
        }

        public StaticValue(TValue value)
        {
            _index = Pop();
            this.Value = value;
        }

        public void Dispose()
        {
            Push(_index);

            if (this.Value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public void SetValue(TValue value)
        {
            this.Value = value;
        }

        public static implicit operator TValue(StaticValue<TNamespace, TValue> value)
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
