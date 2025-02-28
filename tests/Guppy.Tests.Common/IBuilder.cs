namespace Guppy.Tests.Common
{
    public interface IBuilder
    {
        public object Object { get; }
    }

    public interface IBuilder<out T> : IBuilder
    {
        public new T Object { get; }
    }
}
