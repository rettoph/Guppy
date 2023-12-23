using Guppy.Common.Attributes;
using Guppy.Enums;
using Guppy.Resources.Constants;
using System.Runtime.InteropServices;

namespace Guppy.Resources.Providers
{
    [Sequence<InitializeSequence>(InitializeSequence.PreInitialize)]
    internal class ResourceProvider : GlobalComponent, IResourceProvider
    {
        private ISettingProvider _settings;
        private IResourcePackProvider _packs;
        private Dictionary<Resource, IResourceValue> _cache;
        private SettingValue<string> _localization;

        public ResourceProvider(ISettingProvider settings, IResourcePackProvider packs)
        {
            _settings = settings;
            _packs = packs;
            _cache = new Dictionary<Resource, IResourceValue>();
            _localization = _settings.Get(Settings.Localization);
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _packs.Initialize(components);

            foreach (IResourceValue resourceValue in _cache.Values)
            {
                resourceValue.ForceUpdate(this);
            }
        }

        public ResourceValue<T> Get<T>(Resource<T> resource)
            where T : notnull
        {
            ref IResourceValue? cachedValue = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, resource, out bool exists);
            if (exists)
            {
                return (ResourceValue<T>)cachedValue!;
            }

            cachedValue = new ResourceValue<T>(resource);

            if (cachedValue is ResourceValue<T> casted)
            {
                if (this.Ready == false)
                {
                    return casted;
                }

                cachedValue.ForceUpdate(this);

                return casted;
            }

            throw new NotImplementedException();
        }

        internal T GetPackValue<T>(Resource<T> resource)
            where T : notnull
        {
            // TODO: Load default somehow
            List<T> valuesToCache = new List<T>();
            foreach (ResourcePack pack in _packs.GetAll())
            {
                if (pack.TryGet(resource, _localization.Setting.DefaultValue, out T? packValue))
                {
                    valuesToCache.Add(packValue);
                }
            }

            if (_localization != _localization.Setting.DefaultValue)
            {
                foreach (ResourcePack pack in _packs.GetAll())
                {
                    if (pack.TryGet(resource, _localization, out T? packValue))
                    {
                        valuesToCache.Add(packValue);
                    }
                }
            }

            return valuesToCache.LastOrDefault() ?? throw new NotImplementedException();
        }

        public IEnumerable<(Resource, T)> GetAll<T>() where T : notnull
        {
            IEnumerable<Resource<T>> resources = _packs.GetAll().Select(x => x.GetAll<T>()).SelectMany(x => x).Distinct();

            foreach (Resource<T> resource in resources)
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
    }
}
