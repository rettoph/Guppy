
using System.Reflection;
using System.Runtime.InteropServices;

namespace Guppy.Core.StateMachine.Common
{
    public static class StateKey
    {
        public const string DefaultValue = nameof(DefaultValue);

        private static Dictionary<Type, MethodInfo> _createMethodInfos = new Dictionary<Type, MethodInfo>();
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

        public static StateKey<T> Create()
        {
            return new StateKey<T>(StateKey.DefaultValue);
        }

        public static StateKey<T> Create(string value)
        {
            return new StateKey<T>(value);
        }

        public static StateKey<T> Create<TValue>()
        {
            return new StateKey<T>(typeof(TValue).Name);
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

        public bool Equals(Type type, string value)
        {
            return this.Type == type && this.Value == value;
        }

        public bool Equals(IStateKey key)
        {
            return this.Equals(key.Type, key.Value);
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
