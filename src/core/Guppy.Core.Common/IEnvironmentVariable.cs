namespace Guppy.Core.Common
{
    public interface IEnvironmentVariable
    {
        bool Matches(object value);
    }

    public interface IEnvironmentVariable<TKey> : IEnvironmentVariable
        where TKey : IEnvironmentVariable<TKey>
    {
        bool Matches(TKey value);
    }

    public interface IEnvironmentVariable<TKey, TValue> : IEnvironmentVariable<TKey>
        where TKey : IEnvironmentVariable<TKey, TValue>
    {
        static abstract TKey Create(TValue value);
    }
}
