using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Definitions.ResourcePacks
{
    [AutoLoad]
    internal sealed class GuppyResourcePack : ResourcePackDefinition
    {
        public override string Name => nameof(Guppy);

        public override string Path => $"Content\\Guppy";
    }
}
