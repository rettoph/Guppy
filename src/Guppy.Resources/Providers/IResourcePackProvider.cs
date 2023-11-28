using Guppy.Files;
using Guppy.Files.Enums;
using Guppy.Resources.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
