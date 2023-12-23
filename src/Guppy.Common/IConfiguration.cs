namespace Guppy.Common
{
    public interface IConfiguration<T>
        where T : new()
    {
        public T Value { get; }
    }
}
