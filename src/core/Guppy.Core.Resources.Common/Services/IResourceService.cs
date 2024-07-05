namespace Guppy.Core.Resources.Common.Services
{
    public interface IResourceService
    {
        void Initialize();

        IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull;

        ResourceValue<T> GetValue<T>(Resource<T> resource)
            where T : notnull;

        IEnumerable<ResourceValue<T>> GetValues<T>()
            where T : notnull;
    }
}
