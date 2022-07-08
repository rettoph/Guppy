using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Definitions
{
    internal sealed class RuntimeResourceDefinition<T> : ResourceDefinition<T>
    {
        public override string Name { get; }

        public override string? Source { get; }

        public RuntimeResourceDefinition(string name, string? source)
        {
            this.Name = name;
            this.Source = source;
        }
    }
}
