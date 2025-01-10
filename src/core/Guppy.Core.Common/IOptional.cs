namespace Guppy.Core.Common
{
    public interface IOptional<T>
        where T : class
    {
        bool HasValue => this.Value is not null;
        public T? Value { get; }
    }
}