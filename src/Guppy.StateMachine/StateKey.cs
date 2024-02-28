
namespace Guppy.StateMachine
{
    public class StateKey<T> : IStateKey<T>, IEquatable<StateKey<T>?>
    {
        public Type Type { get; }
        public string Value { get; }

        private StateKey(string key)
        {
            this.Type = typeof(T);
            this.Value = key;
        }

        public static StateKey<T> Create()
        {
            return new StateKey<T>(typeof(T).AssemblyQualifiedName ?? throw new Exception());
        }

        public static StateKey<T> Create(string value)
        {
            return new StateKey<T>(value);
        }

        public static StateKey<T> Create<TValue>()
        {
            return new StateKey<T>(typeof(TValue).AssemblyQualifiedName ?? throw new Exception());
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as StateKey<T>);
        }

        public bool Equals(StateKey<T>? other)
        {
            return other is not null &&
                   EqualityComparer<Type>.Default.Equals(Type, other.Type) &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }

        public static bool operator ==(StateKey<T>? left, StateKey<T>? right)
        {
            return EqualityComparer<StateKey<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(StateKey<T>? left, StateKey<T>? right)
        {
            return !(left == right);
        }
    }
}
