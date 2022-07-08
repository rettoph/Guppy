using Guppy.Resources.Constants;
using Guppy.Resources.Definitions;
using Guppy.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal sealed class ResourcePackStringProvider : IResourcePackTypeProvider
    {
        private readonly ISettingProvider _settings;
        private readonly ISetting<string> _lang;
        private Dictionary<string, Dictionary<string, Resource<string>>> _strings;

        public int Order => 0;

        public ResourcePackStringProvider(ISettingProvider settings)
        {
            _settings = settings;

            _lang = _settings.Get<string>(SettingConstants.CurrentLanguage);

            _strings = null!;
        }

        public void Load(IResourcePack pack, IEnumerable<IResourceDefinition> resources)
        {
            Dictionary<string, List<Resource<string>>> strings = new Dictionary<string, List<Resource<string>>>();

            foreach(string path in pack.SearchForFiles("*lang*.json"))
            {
                using(FileStream file = File.OpenRead(path))
                {
                    Strings? fileStrings = JsonSerializer.Deserialize<Strings>(file);

                    if(fileStrings is null || fileStrings.LanguageIdentifier is null || fileStrings.Values is null)
                    {
                        continue;
                    }

                    if(!strings.ContainsKey(fileStrings.LanguageIdentifier))
                    {
                        strings.Add(fileStrings.LanguageIdentifier, new List<Resource<string>>());
                    }

                    foreach(var kvp in fileStrings.Values)
                    {
                        if(resources.Any(x => x.Name == kvp.Key))
                        {
                            strings[fileStrings.LanguageIdentifier].Add(new Resource<string>(kvp.Value, kvp.Key, path, pack));
                        }
                    }
                }
            }

            _strings = strings.ToDictionary(x => x.Key, x => x.Value.ToDictionary(s => s.Name));
        }

        public IResource? Get(string name)
        {
            if(_strings.TryGetValue(_lang.Value, out var language))
            {
                if(language.TryGetValue(name, out var resource))
                {
                    return resource;
                }
            }

            return null;
        }

        public bool Provides(Type type)
        {
            return type == typeof(string);
        }
    }
}
