namespace Guppy.Core.Resources.Common.Services
{
    public interface IResourceService
    {
        void Initialize();

        IEnumerable<ResourceKey<T>> GetKeys<T>()
            where T : notnull;

        Resource<T> Get<T>(ResourceKey<T> resource)
            where T : notnull;

        IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull;
    }
}
