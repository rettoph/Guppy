using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Definitions
{
    public abstract class ResourcePackDefinition : IResourcePackDefinition
    {
        public abstract string Name { get; }

        public abstract string Path { get; }

        public IResourcePack BuildResourcePack(IEnumerable<IResourceDefinition> resources, IEnumerable<IResourcePackTypeProvider> providers)
        {
            return new ResourcePack(this.Name, this.Path, resources, providers);
        }
    }
}
