using Guppy.Resources.Definitions;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    internal sealed class ResourcePack : IResourcePack
    {
        private Dictionary<Type, IResourcePackTypeProvider> _providers;

        public string Name { get; }

        public string Path { get; }

        public ResourcePack(string name, string path, IEnumerable<IResourceDefinition> resources, IEnumerable<IResourcePackTypeProvider> providers)
        {
            this.Name = name;
            this.Path = path;

            var types = resources.Select(x => x.Type).Distinct().ToList();

            _providers = new Dictionary<Type, IResourcePackTypeProvider>();

            foreach(Type type in types)
            {
                var provider = providers.First(p => p.Provides(type));

                if(provider.Load(this, resources.Where(x => x.Type == type), true))
                {
                    _providers.Add(type, provider);
                }
            }
        }

        public bool TryGet<T>(string name, [MaybeNullWhen(false)] out IResource<T> value)
        {
            if(_providers.TryGetValue(typeof(T), out var provider) && provider.Get(name) is IResource<T> resource)
            {
                value = resource;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerable<string> SearchForFiles(string pattern)
        {
            return Directory.GetFiles(this.Path, pattern);
        }
    }
}
