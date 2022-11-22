using Guppy.Resources.Constants;
using Guppy.Resources.Definitions;
using Guppy.Resources.Models;
using Guppy.Resources.Serialization.Json;
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

        public ResourcePackStringProvider(ISettingProvider settings, IJsonSerializer serializer)
        {
            _settings = settings;

            _lang = _settings.Get<string>(SettingConstants.CurrentLanguage);

            _strings = null!;
        }

        public bool Load(IResourcePack pack, IEnumerable<IResourceDefinition> resources, bool strict)
        {
            Dictionary<string, List<Resource<string>>> strings = new Dictionary<string, List<Resource<string>>>();
            var any = false;

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
                        if(resources.Any(x => x.Name == kvp.Key) || !strict)
                        {
                            any = true;
                            strings[fileStrings.LanguageIdentifier].Add(new Resource<string>(kvp.Value, kvp.Key, path, pack));
                        }
                    }
                }
            }

            if(!any)
            {
                return false;
            }

            _strings = strings.ToDictionary(x => x.Key, x => x.Value.ToDictionary(s => s.Name));
            return true;
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
