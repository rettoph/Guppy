using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Definitions
{
    public interface IResourcePackDefinition
    {
        string Name { get; }
        string Path { get; }

        IResourcePack BuildResourcePack(IEnumerable<IResourceDefinition> resources, IEnumerable<IResourcePackTypeProvider> providers);
    }
}
