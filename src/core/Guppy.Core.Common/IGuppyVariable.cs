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
    public interface IGuppyVariable<TSelf, TValue> : IGuppyVariable<TSelf>
        where TSelf : IGuppyVariable<TSelf, TValue>
    {
        static abstract TSelf Create(TValue value);
    }
}
