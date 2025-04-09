using System.Runtime.InteropServices;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Logging.Common;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Services;

namespace Guppy.Core.Assets.Services
{
    public class AssetService(Lazy<IAssetPackService> packs, Lazy<ILogger<AssetService>> logger) : IAssetService, IDisposable
    {
        private bool _initialized;
        private readonly Lazy<IAssetPackService> _resourcePackService = packs;
        private readonly Lazy<ILogger<AssetService>> _logger = logger;

        private readonly Dictionary<Guid, IAsset> _values = [];

        public void Dispose()
        {
            foreach (IAsset resourceValue in this._values.Values)
            {
                resourceValue.Dispose();
            }

            this._values.Clear();
        }

        public void Initialize()
        {
            if (this._initialized)
            {
                return;
            }

            this._initialized = true;

            this._logger.Value.Debug("Preparing to build resource value dictionary");

            foreach (IAssetKey resourceKey in this._resourcePackService.Value.GetDefinedAssets())
            {
                this.CacheGetOrAddValues(resourceKey).Refresh(this._resourcePackService.Value);
            }

            this._logger.Value.Debug("Done. Found ({Count}) resources", this._values.Count);
            foreach (IAsset value in this._values.Values)
            {
                this._logger.Value.Verbose("Asset = {Asset}, Type = {Type}, Value = {Value}, Count = {Count}", value.Key.Name, value.Key.Type.GetFormattedName(), value.Value, value.All().Count());
            }
        }

        public Asset<T> Get<T>(AssetKey<T> resource)
            where T : notnull
        {
            return (Asset<T>)this.CacheGetOrAddValues(resource);
        }

        public IEnumerable<Asset<T>> GetAll<T>() where T : notnull
        {
            return AssetKey<T>.GetAll().Select(x => this.Get(x));
        }

        private IAsset CacheGetOrAddValues(IAssetKey key)
        {
            ref IAsset? cache = ref CollectionsMarshal.GetValueRefOrAddDefault(this._values, key.Id, out bool exists);
            if (exists == true)
            {
                return cache!;
            }

            cache = key.CreateAsset();

            if (this._initialized == false)
            {
                return cache;
            }

            cache.Refresh(this._resourcePackService.Value);
            return cache;
        }

        public IEnumerable<AssetKey<T>> GetKeys<T>()
            where T : notnull
        {
            return AssetKey<T>.GetAll();
        }
    }
}