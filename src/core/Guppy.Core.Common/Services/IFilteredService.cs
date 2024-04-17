namespace Guppy.Core.Common.Services
{
    public interface IFilteredService
    {
        IFiltered<T> Get<T>()
            where T : class;
    }
}
