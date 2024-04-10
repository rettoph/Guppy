namespace Guppy.Common.Services
{
    public interface IFilteredService
    {
        IFiltered<T> Get<T>()
            where T : class;

        T? Instance<T>()
            where T : class;

        IEnumerable<T> Instances<T>()
            where T : class;
    }
}
