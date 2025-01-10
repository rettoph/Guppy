namespace Guppy.Core.Common
{
    public interface IFiltered<T> : IEnumerable<T>
        where T : class
    {
    }
}