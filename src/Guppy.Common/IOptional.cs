namespace Guppy.Common
{
    public interface IOptional<T>
        where T : class
    {
        bool HasValue => Value is not null;
        public T? Value { get; }
    }
}
