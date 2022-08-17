using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS
{
    public record EntityKey(string Name)
    {
        public readonly uint Id = Name.xxHash();
    }
}
