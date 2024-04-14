namespace Guppy.StateMachine
{
    public class State<T> : IState<T>
    {
        private readonly Func<T?> _valueGetter;
        private readonly Func<T?, T?, bool> _comparer;

        public IStateKey<T> Key { get; }

        public T? Value => _valueGetter();

        IStateKey IState.Key => this.Key;

        object? IState.Value => this.Value;

        public State(T? value) : this(StateKey<T>.Create(), value, DefaultCompare)
        {

        }
        public State(T? value, Func<T?, T?, bool> comparer) : this(StateKey<T>.Create(), value, comparer)
        {

        }
        public State(IStateKey<T> key, T? value) : this(key, value, DefaultCompare)
        {

        }
        public State(IStateKey<T> key, T? value, Func<T?, T?, bool> comparer) : this(key, () => value, comparer)
        {
        }
        public State(Func<T?> valueGetter) : this(StateKey<T>.Create(), valueGetter, DefaultCompare)
        {

        }
        public State(Func<T?> valueGetter, Func<T?, T?, bool> comparer) : this(StateKey<T>.Create(), valueGetter, comparer)
        {

        }
        public State(IStateKey<T> key, Func<T?> valueGetter) : this(key, valueGetter, DefaultCompare)
        {

        }
        public State(IStateKey<T> key, Func<T?> valueGetter, Func<T?, T?, bool> comparer)
        {
            _valueGetter = valueGetter;
            _comparer = comparer;

            this.Key = key;
        }

        public bool Equals(T? other)
        {
            return _comparer(this.Value, other);
        }

        private static bool DefaultCompare(T? value, T? other)
        {
            if (value is null)
            {
                return other is null;
            }

            return value.Equals(other);
        }
    }
}
