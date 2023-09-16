using Guppy.Common.Collections;
using Guppy.Resources.Constants;
using Guppy.Resources.Loaders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal class ResourceProvider : IResourceProvider
    {
        private ISettingProvider _settings;
        private Lazy<IResourcePackProvider> _packs;
        private Dictionary<Resource, object> _cache;
        private ISetting<string> _localization;

        public ResourceProvider(ISettingProvider settings, Lazy<IResourcePackProvider> packs)
        {
            _settings = settings;
            _packs = packs;
            _cache = new Dictionary<Resource, object>();
            _localization = _settings.Get<string>(SettingConstants.Localization);
        }

        public T Get<T>(Resource<T> resource) where T : notnull
        {
            ref T cache = ref this.GetCache(resource);
            return cache;
        }

        public bool TryGet<T>(Resource<T> resource, [MaybeNullWhen(false)] out T value) 
            where T : notnull
        {
            ref T cache = ref this.GetCache(resource);

            value = (T)cache;
            return cache is not null;
        }

        public IEnumerable<(Resource, T)> GetAll<T>() where T : notnull
        {
            IEnumerable<Resource<T>> resources = _packs.Value.GetAll().Select(x => x.GetAll<T>()).SelectMany(x => x).Distinct();


            foreach(Resource<T> resource in resources)
            {
                yield return (resource, this.Get(resource));
            }
        }

        public IResourceProvider Set<T>(Resource<T> resource, T value) where T : notnull
        {
            throw new NotImplementedException();

            // ref T[] cache = ref this.GetCache(resource);
            // Array.Resize(ref cache, cache.Length + 1);
            // _cache[resource] = value;
            // 
            // return this;
        }

        private ref T GetCache<T>(Resource<T> resource)
            where T : notnull
        {
            ref object? cache = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, resource, out bool exists);
            if (exists)
            {
                return ref Unsafe.As<object?, T>(ref cache);
            }

            List<T> valuesToCache = new List<T>();
            foreach (ResourcePack pack in _packs.Value.GetAll())
            {
                if (pack.TryGet(resource, Localization.Default, out T? value))
                {
                    valuesToCache.Add(value);
                }
            }

            if(_localization.Value != Localization.Default)
            {
                foreach (ResourcePack pack in _packs.Value.GetAll())
                {
                    if (pack.TryGet(resource, _localization.Value, out T? value))
                    {
                        valuesToCache.Add(value);
                    }
                }
            }

            cache = valuesToCache.LastOrDefault();

            if(cache == null)
            {
                // TODO: Load default somehow
            }

            return ref Unsafe.As<object?, T>(ref cache);
        }
    }
}
