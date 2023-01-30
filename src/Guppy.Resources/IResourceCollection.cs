using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface IResourceCollection
    {
        IEnumerable<IResource> Items { get; }

        void Add(IResource resource);
        bool TryGet<T>(string name, [MaybeNullWhen(false)] out IResource<T> resource);
    }
}
