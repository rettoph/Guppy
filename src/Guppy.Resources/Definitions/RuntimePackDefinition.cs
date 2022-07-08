using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Definitions
{
    internal sealed class RuntimePackDefinition : ResourcePackDefinition
    {
        public override string Name { get; }

        public override string Path { get; }

        public RuntimePackDefinition(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }
    }
}
