using Guppy.Resources;
using Guppy.Resources.Definitions;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers.ResourcePackTypeProviders
{
    internal sealed class ResourcePackTrueTypeFontProvider : IResourcePackTypeProvider
    {
        private Dictionary<string, Resource<TrueTypeFont>> _fonts;

        public ResourcePackTrueTypeFontProvider()
        {
            _fonts = new Dictionary<string, Resource<TrueTypeFont>>();
        }

        public IResource? Get(string name)
        {
            if(_fonts.TryGetValue(name, out var resource))
            {
                return resource;
            }

            return null;
        }

        public bool Load(IResourcePack pack, IEnumerable<IResourceDefinition> resources, bool strict)
        {
            var fonts = new List<Resource<TrueTypeFont>>();

            foreach(IResourceDefinition resource in resources)
            {
                if(resource.Source is null)
                {
                    continue;
                }

                var fullPath = Path.Combine(pack.Path, resource.Source);

                if(File.Exists(fullPath))
                {
                    using (var stream = File.OpenRead(fullPath))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);

                        fonts.Add(new Resource<TrueTypeFont>(new TrueTypeFont(buffer), resource.Name, fullPath, pack));
                    }
                }
            }

            if(!fonts.Any())
            {
                return false;
            }

            _fonts = fonts.ToDictionary(x => x.Name);
            return true;
        }

        public bool Provides(Type type)
        {
            return type == typeof(TrueTypeFont);
        }
    }
}
