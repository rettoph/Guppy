namespace Guppy.Core.Resources.Common.Services
{
    public interface IResourcePackService
    {
        void Initialize();
        IEnumerable<ResourcePack> GetAll();
        ResourcePack GetById(Guid id);

        IEnumerable<IResource> GetDefinedResources();
        IEnumerable<T> GetDefinedValues<T>(Resource<T> resource) where T : notnull;
    }
}
