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
        private IResourcePackProvider _packs;

        public ResourceProvider(IResourcePackProvider packs)
        {
            _packs = packs;
        }

        public T Get<T>(string name)
        {
            foreach(IResourcePack pack in _packs)
            {
                if(pack.TryGet<T>(name, out var resource))
                {
                    return resource.Value;
                }
            }

            return default(T) ?? throw new ArgumentException();
        }

        public bool TryGet<T>(string name, [MaybeNullWhen(false)] T resource)
        {
            foreach (IResourcePack pack in _packs)
            {
                if (pack.TryGet<T>(name, out var r))
                {
                    resource = r.Value;
                    return true;
                }
            }

            resource = default;
            return false;
        }
    }
}
