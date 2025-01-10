
using System.Reflection;
using System.Runtime.InteropServices;

namespace Guppy.Core.StateMachine.Common
{
    public static class StateKey
    {
        public const string DefaultValue = nameof(DefaultValue);

        private static readonly Dictionary<Type, MethodInfo> _createMethodInfos = [];
        private static MethodInfo GetCreateMethodInfo(Type type)
        {
            ref MethodInfo? methodInfo = ref CollectionsMarshal.GetValueRefOrAddDefault(_createMethodInfos, type, out bool exists);
            if (exists)
            {
                return methodInfo ?? throw new NotImplementedException();
            }

            methodInfo = typeof(StateKey<>).MakeGenericType(type).GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.Public, [typeof(string)]);
            return methodInfo ?? throw new NotImplementedException();
        }

        public static IStateKey Create(Type type, string value)
        {
            MethodInfo createMethod = GetCreateMethodInfo(type);
            object? stateKeyInstance = createMethod.Invoke(null, [value]);

            return stateKeyInstance as IStateKey ?? throw new NotImplementedException();
        }
    }

    public class StateKey<T> : IStateKey<T>, IEquatable<StateKey<T>?>
    {
        public static readonly IStateKey<T> Default = StateKey<T>.Create();

        public Type Type { get; }
        public string Value { get; }

        private StateKey(string key)
        {
            this.Type = typeof(T);
            this.Value = key;
        }

        public static StateKey<T> Create() => new(StateKey.DefaultValue);

        public static StateKey<T> Create(string value) => new(value);

        public static StateKey<T> Create<TValue>() => new(typeof(TValue).Name);

        public override bool Equals(object? obj) => this.Equals(obj as StateKey<T>);

        public bool Equals(StateKey<T>? other) => other is not null &&
                   EqualityComparer<Type>.Default.Equals(this.Type, other.Type) &&
                   this.Value == other.Value;

        public override int GetHashCode() => HashCode.Combine(this.Type, this.Value);

        public bool Equals(Type type, string value) => this.Type == type && this.Value == value;

        public bool Equals(IStateKey key) => this.Equals(key.Type, key.Value);

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