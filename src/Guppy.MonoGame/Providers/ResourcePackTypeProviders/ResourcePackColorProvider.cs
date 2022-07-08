using Guppy.Resources;
using Guppy.Resources.Constants;
using Guppy.Resources.Definitions;
using Guppy.Resources.Models;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Providers.ResourcePackTypeProviders
{
    internal sealed class ResourcePackColorProvider : IResourcePackTypeProvider
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private Dictionary<string, Resource<Color>> _colors;

        public int Order => 0;

        public ResourcePackColorProvider(JsonSerializerOptions jsonOptions)
        {
            _jsonOptions = jsonOptions;
            _colors = null!;
        }

        public void Load(IResourcePack pack, IEnumerable<IResourceDefinition> resources)
        {
            List<Resource<Color>> colors = new List<Resource<Color>>();

            foreach(string path in pack.SearchForFiles("*colors*.json"))
            {
                using(FileStream file = File.OpenRead(path))
                {
                    Dictionary<string, Color>? fileColors = JsonSerializer.Deserialize<Dictionary<string, Color>>(file, _jsonOptions);

                    if(fileColors is null)
                    {
                        continue;
                    }

                    foreach(var kvp in fileColors)
                    {
                        if(resources.Any(x => x.Name == kvp.Key))
                        {
                            colors.Add(new Resource<Color>(kvp.Value, kvp.Key, path, pack));
                        }
                    }
                }
            }

            _colors = colors.ToDictionary(x => x.Name);
        }

        public IResource? Get(string name)
        {
            if(_colors.TryGetValue(name, out var resource))
            {
                return resource;
            }

            return null;
        }

        public bool Provides(Type type)
        {
            return type == typeof(Color);
        }
    }
}
