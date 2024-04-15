namespace Guppy.Core.Resources.Common.Services
{
    public interface IResourcePackService
    {
        void Initialize();
        IEnumerable<ResourcePack> GetAll();
        ResourcePack GetById(Guid id);
    }
}
