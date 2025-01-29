using System.ComponentModel;

namespace Guppy.Core.Common
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IGuppyVariable
    {
        bool Matches(object value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IGuppyVariable<TKey> : IGuppyVariable
        where TKey : IGuppyVariable<TKey>
    {
        bool Matches(TKey value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IGuppyVariable<TKey, TValue> : IGuppyVariable<TKey>
        where TKey : IGuppyVariable<TKey, TValue>
    {
        static abstract TKey Create(TValue value);
    }
}
