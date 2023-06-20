using Guppy.Resources.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal class ResourceProvider : IResourceProvider
    {
        private ISettingProvider _settings;
        private IResourcePackProvider _packs;
        private Dictionary<Resource, object> _cache;
        private ISetting<string> _localization;

        public ResourceProvider(ISettingProvider settings, IResourcePackProvider packs)
        {
            _settings = settings;
            _packs = packs;
            _cache = new Dictionary<Resource, object>();
            _localization = _settings.Get<string>(SettingConstants.Localization);
        }

        public T? Get<T>(Resource<T> resource) where T : notnull
        {
            this.TryGet(resource, out T? value);
            return value;
        }

        public bool TryGet<T>(Resource<T> resource, [MaybeNullWhen(false)] out T value) 
            where T : notnull
        {
            if(_cache.TryGetValue(resource, out object? cached))
            {
                value = (T)cached;
                return true;
            }

            foreach (ResourcePack pack in _packs.GetAll())
            {
                if(pack.TryGet(resource, _localization.Value, out value))
                {
                    _cache.Add(resource, value);
                    return true;
                }

                if (_localization.Value != Localization.Default && pack.TryGet(resource, Localization.Default, out value))
                {
                    _cache.Add(resource, value);
                    return true;
                }
            }

            value = default!;
            return false;
        }

        public IEnumerable<(Resource, T)> GetAll<T>() where T : notnull
        {
            List<(Resource, T)> resources = new List<(Resource, T)>();
            foreach((Resource resource, object cached)  in _cache)
            {
                if(cached is T casted)
                {
                    resources.Add((resource, casted));
                }
            }

            return resources;
        }

        public IResourceProvider Set<T>(Resource<T> resource, T value) where T : notnull
        {
            _cache[resource] = value;

            return this;
        }
    }
}
