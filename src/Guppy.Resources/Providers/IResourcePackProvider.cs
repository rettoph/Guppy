namespace Guppy.Resources.Providers
{
    public interface IResourcePackProvider : IGlobalComponent
    {
        IEnumerable<ResourcePack> GetAll();
        ResourcePack GetById(Guid id);
    }
}
