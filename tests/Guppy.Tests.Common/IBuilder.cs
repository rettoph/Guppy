namespace Guppy.Tests.Common
{
    public interface IBuilder<out T>
    {
        public T Object { get; }
    }
}
