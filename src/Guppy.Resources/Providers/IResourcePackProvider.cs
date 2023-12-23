using Guppy.Files;
using Guppy.Files.Enums;
using Guppy.Resources.Configuration;

namespace Guppy.Resources.Providers
{
    public interface IResourcePackProvider : IGlobalComponent
    {
        void Register(IFile<ResourcePackConfiguration> options);
        void Register(FileType type, string path);
        IEnumerable<ResourcePack> GetAll();
        ResourcePack GetById(Guid id);
    }
}
