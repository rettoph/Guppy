using Guppy.Files.Enums;

namespace Guppy.Resources.Providers
{
    public interface IResourcePackProvider : IGlobalComponent
    {
        void Register(FileType type, string path);
        IEnumerable<ResourcePack> GetAll();
        ResourcePack GetById(Guid id);
    }
}
