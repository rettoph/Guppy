using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Definitions
{
    public abstract class ResourceDefinition<T> : IResourceDefinition
    {
        public abstract string Name { get; }

        public abstract string? Source { get; }

        public Type Type => typeof(T);
    }
}
