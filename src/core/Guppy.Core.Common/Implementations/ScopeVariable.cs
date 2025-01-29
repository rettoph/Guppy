namespace Guppy.Core.Common.Implementations
{
    public abstract class ScopeVariable<TKey, TValue>(TValue value) : GuppyVariable<TKey, TValue>(value), IScopeVariable<TKey, TValue>
        where TKey : ScopeVariable<TKey, TValue>
    {
    }
}
