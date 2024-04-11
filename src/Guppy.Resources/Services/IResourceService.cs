namespace Guppy.Resources.Services
{
    public interface IResourceService : IGlobalComponent
    {
        IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull;

        void RefreshAll();
    }
}
