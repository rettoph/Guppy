using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface IResourcePackProvider
    {
        void Configure(Guid id, Action<ResourcePack> configurator);
        IEnumerable<ResourcePack> GetAll();
        ResourcePack GetById(Guid id);
    }
}
