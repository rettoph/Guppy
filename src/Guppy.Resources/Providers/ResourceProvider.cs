﻿using Guppy.Resources.Constants;
using System;
using System.Collections.Generic;
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
        private IResourcePackProvider _packs;
        private Dictionary<Resource, Array> _cache;
        private ISetting<string> _localization;

        public ResourceProvider(ISettingProvider settings, IResourcePackProvider packs)
        {
            _settings = settings;
            _packs = packs;
            _cache = new Dictionary<Resource, Array>();
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
            ref T[] cache = ref this.GetCache(resource);

            if(cache.Length == 0)
            {
                value = default!;
                return false;
            }

            value = cache[0];
            return true;
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

        public IEnumerable<T> GetAll<T>(Resource<T> resource) 
            where T : notnull
        {
            ref T[] cache = ref this.GetCache(resource);

            return cache;
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

        private ref T[] GetCache<T>(Resource<T> resource)
            where T : notnull
        {
            ref Array? cache = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, resource, out bool exists);
            if (exists)
            {
                return ref Unsafe.As<Array?, T[]>(ref cache);
            }

            List<T> valuesToCache = new List<T>();
            foreach (ResourcePack pack in _packs.GetAll())
            {
                if (pack.TryGet(resource, Localization.Default, out IEnumerable<T> values))
                {
                    valuesToCache.AddRange(values);
                }
            }

            if(_localization.Value != Localization.Default)
            {
                foreach (ResourcePack pack in _packs.GetAll())
                {
                    if (pack.TryGet(resource, _localization.Value, out IEnumerable<T> values))
                    {
                        valuesToCache.AddRange(values);
                    }
                }
            }

            valuesToCache.Reverse();
            cache = valuesToCache.ToArray();

            return ref Unsafe.As<Array?, T[]>(ref cache);
        }
    }
}
