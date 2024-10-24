namespace Guppy.Core.Resources.Common.Services
{
    public interface IResourcePackService
    {
        void Initialize();
        IEnumerable<IResourcePack> GetAll();
        IResourcePack GetById(Guid id);

        IEnumerable<IResourceKey> GetDefinedResources();
        IEnumerable<T> GetDefinedValues<T>(ResourceKey<T> resource) where T : notnull;
    }
}
