using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface IResource
    {
        string Name { get; }
        string? Source { get; }
        Type Type { get; }
        IResourcePack Pack { get; }
    }

    public interface IResource<T> : IResource
    {
        T Value { get; }
    }
}
