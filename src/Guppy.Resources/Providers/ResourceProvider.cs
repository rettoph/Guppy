using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal class ResourceProvider : IResourceProvider
    {
        private IPackProvider _packs;
        private IDictionary<string, IResource> _cache;

        public ResourceProvider(IPackProvider packs)
        {
            _packs = packs;
            _cache = new Dictionary<string, IResource>();
        }

        public IResource<T> Get<T>(string name)
        {
            if(_cache.TryGetValue(name, out var uncasted) && uncasted is IResource<T> casted)
            {
                return casted;
            }

            foreach(Pack pack in _packs.GetAll())
            {
                if(pack.TryGet<T>(name, out var resource))
                {
                    _cache.Add(name, resource);
                    return resource;
                }
            }

            return default(IResource<T>) ?? throw new ArgumentException();
        }

        public bool TryGet<T>(string name, [MaybeNullWhen(false)] IResource<T> resource)
        {
            if (_cache.TryGetValue(name, out var uncasted) && uncasted is IResource<T> casted)
            {
                resource = casted;
                return true;
            }

            foreach (Pack pack in _packs.GetAll())
            {
                if (pack.TryGet<T>(name, out var r))
                {
                    _cache.Add(name, r);
                    resource = r;
                    return true;
                }
            }

            resource = default;
            return false;
        }

        public IEnumerable<T> GetAll<T>()
            where T : IResource
        {
            var output = new List<T>();

            foreach(Pack pack in _packs.GetAll())
            {
                output.AddRange(pack.Resources.WhereAs<IResource, T>());
            }

            return output;
        }
    }
}
