namespace Guppy.Resources.Services
{
    public interface IResourcePackService : IGlobalComponent
    {
        IEnumerable<ResourcePack> GetAll();
        ResourcePack GetById(Guid id);
    }
}
