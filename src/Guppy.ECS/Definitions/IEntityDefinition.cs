using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    public interface IEntityDefinition
    {
        EntityKey Key { get; }

        EntityTag[] Tags { get; }
    }
}
