using Guppy.Resources;
using Guppy.Resources.Definitions;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Providers.ResourcePackTypeProviders
{
    public sealed class ResourcePackContentProvider<T> : IResourcePackTypeProvider
    {
        private ContentManager _manager;
        private Dictionary<string, ContentResource<T>> _content;

        public ResourcePackContentProvider(ContentManager manager)
        {
            _manager = manager;
            _content = null!;
        }

        public IResource? Get(string name)
        {
            if(_content.TryGetValue(name, out var resource))
            {
                return resource;
            }

            return default;
        }

        public bool Load(IResourcePack pack, IEnumerable<IResourceDefinition> resources, bool strict)
        {
            _manager.RootDirectory = pack.Path;

            var content = new List<ContentResource<T>>();
            var any = false;

            foreach(IResourceDefinition resource in resources)
            {
                if (resource.Source is null)
                {
                    continue;
                }
                    
                var fullSource = Path.Combine(pack.Path, resource.Source);

                if (pack.SearchForFiles($"{resource.Source}.*").Any())
                {
                    any = true;
                    content.Add(new ContentResource<T>(
                        _manager.Load<T>(resource.Source),
                        resource.Name,
                        fullSource,
                        pack));
                }
            }

            if(!any)
            {
                return false;
            }

            _content = content.ToDictionary(x => x.Name);
            return true;
        }

        public bool Provides(Type type)
        {
            return type == typeof(T);
        }
    }
}
