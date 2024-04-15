namespace Guppy.Core.Resources.Common.Services
{
    public interface IResourceService
    {
        void Initialize();

        IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull;

        T GetLatestValue<T>(Resource<T> resource)
            where T : notnull;

        void RefreshAll();
    }
}
