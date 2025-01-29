namespace Guppy.Core.Common.Implementations
{
    public abstract class EnvironmentVariable<TKey, TValue>(TValue value) : GuppyVariable<TKey, TValue>(value), IEnvironmentVariable<TKey, TValue>
        where TKey : EnvironmentVariable<TKey, TValue>
        where TValue : notnull
    {
        public static implicit operator KeyValuePair<Type, IEnvironmentVariable>(EnvironmentVariable<TKey, TValue> variable)
        {
            return new KeyValuePair<Type, IEnvironmentVariable>(typeof(TKey), variable);
        }
    }
}
