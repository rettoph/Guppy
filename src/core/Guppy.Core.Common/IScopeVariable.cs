namespace Guppy.Core.Common
{
    public interface IScopeVariable : IGuppyVariable
    {
    }

    public interface IScopeVariable<TKey> : IScopeVariable, IGuppyVariable<TKey>
        where TKey : IGuppyVariable<TKey>
    {
    }

    public interface IScopeVariable<TKey, TValue> : IScopeVariable<TKey>, IGuppyVariable<TKey, TValue>
        where TKey : IGuppyVariable<TKey, TValue>
    {
    }
}
