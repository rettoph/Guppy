namespace Guppy.Core.Common
{
    public interface IRef
    {
        Type Type { get; }
        object? Value { get; }
    }

    public interface IRef<T> : IRef
    {
        new T Value { get; set; }
    }
}