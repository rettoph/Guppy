using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public class Resource<T> : IResource<T>
    {
        public T Value { get; }

        public string Name { get; }

        public string? Source { get; }

        public Type Type => typeof(T);

        public IResourcePack Pack { get; }

        public Resource(T value, string name, string? source, IResourcePack pack)
        {
            this.Value = value;
            this.Name = name;
            this.Pack = pack;
            this.Source = source;
        }
    }
}
