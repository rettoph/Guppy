using Guppy.Resources.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal sealed class ResourcePackProvider : IResourcePackProvider
    {
        private Dictionary<string, IResourcePack> _packs;

        public ResourcePackProvider(IServiceProvider provider, IEnumerable<IResourceDefinition> resources, IEnumerable<IResourcePackDefinition> definitions)
        {
            _packs = definitions.Select(x => x.BuildResourcePack(resources, provider.GetServices<IResourcePackTypeProvider>())).ToDictionary(x => x.Name);
        }

        public bool TryGet(string name, [MaybeNullWhen(false)] IResourcePack pack)
        {
            return _packs.TryGetValue(name, out var result);
        }

        public IEnumerator<IResourcePack> GetEnumerator()
        {
            return _packs.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
