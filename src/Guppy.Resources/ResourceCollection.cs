using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    internal sealed class ResourceCollection : IResourceCollection
    {
        private IDictionary<string, IResource> _resources;

        public ResourceCollection(params IResource[] resources)
        {
            _resources = resources.ToDictionary(x => x.Name, x => x);
        }

        public IEnumerable<IResource> Items => _resources.Values;

        public void Add(IResource resource)
        {
            _resources[resource.Name] = resource;
        }

        public bool TryGet<T>(string name, [MaybeNullWhen(false)] out IResource<T> resource)
        {
            if(_resources.TryGetValue(name, out var uncasted) && uncasted is IResource<T> casted)
            {
                resource = casted;
                return true;
            }

            resource = null;
            return false;
        }
    }
}
