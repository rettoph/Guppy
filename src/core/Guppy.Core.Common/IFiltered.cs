namespace Guppy.Core.Common
{
    public interface IFiltered<T>
        where T : class
    {
        public T Instance { get; }
        public IEnumerable<T> Instances { get; }
    }
}
