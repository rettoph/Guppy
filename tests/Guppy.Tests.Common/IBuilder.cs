namespace Guppy.Tests.Common
{
    public interface IBuilder
    {
        object Object { get; }
    }

    public interface IBuilder<out T> : IBuilder
    {
        new T Object { get; }
    }
}
