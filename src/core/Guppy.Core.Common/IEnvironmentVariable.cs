namespace Guppy.Core.Common
{
    public interface IEnvironmentVariable : IGuppyVariable
    {
    }

    public interface IEnvironmentVariable<TKey> : IEnvironmentVariable, IGuppyVariable<TKey>
        where TKey : IGuppyVariable<TKey>
    {
    }

    public interface IEnvironmentVariable<TKey, TValue> : IEnvironmentVariable<TKey>, IGuppyVariable<TKey, TValue>
        where TKey : IGuppyVariable<TKey, TValue>
    {
    }
}
