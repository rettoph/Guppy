namespace Guppy.Resources.Services
{
    public interface IResourceService
    {
        void Initialize();

        IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull;

        void RefreshAll();
    }
}
