using System.Runtime.InteropServices;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Services;
using Guppy.Core.Logging.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Core.Resources.Services
{
    internal class ResourceService(Lazy<IResourcePackService> packs, Lazy<ILogger<ResourceService>> logger) : IHostedService, IResourceService, IDisposable
    {
        private bool _initialized;
        private readonly Lazy<IResourcePackService> _resourcePackService = packs;
        private readonly Lazy<ILogger<ResourceService>> _logger = logger;

        private readonly Dictionary<Guid, IResource> _values = [];

        public void Dispose()
        {
            foreach (IResource resourceValue in this._values.Values)
            {
                resourceValue.Dispose();
            }

            this._values.Clear();
        }

        public Task StartAsync(CancellationToken cancellation)
        {
            this.Initialize();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }

        public void Initialize()
        {
            if (this._initialized)
            {
                return;
            }

            this._resourcePackService.Value.Initialize();

            this._logger.Value.Debug("Preparing to build resource value dictionary");

            foreach (IResourceKey resourceKey in this._resourcePackService.Value.GetDefinedResources())
            {
                this.CacheGetOrAddValues(resourceKey).Refresh(this._resourcePackService.Value);
            }

            this._logger.Value.Debug("Done. Found ({Count}) resources", this._values.Count);
            foreach (IResource value in this._values.Values)
            {
                this._logger.Value.Verbose("Resource = {Resource}, Type = {Type}, Value = {Value}, Count = {Count}", value.Key.Name, value.Key.Type.GetFormattedName(), value.Value, value.All().Count());
            }

            this._initialized = true;
        }

        public Resource<T> Get<T>(ResourceKey<T> resource)
            where T : notnull
        {
            return (Resource<T>)this.CacheGetOrAddValues(resource);
        }

        public IEnumerable<Resource<T>> GetAll<T>() where T : notnull
        {
            return ResourceKey<T>.GetAll().Select(x => this.Get(x));
        }

        private IResource CacheGetOrAddValues(IResourceKey key)
        {

            ref IResource? cache = ref CollectionsMarshal.GetValueRefOrAddDefault(this._values, key.Id, out bool exists);
            if (exists == true)
            {
                return cache!;
            }

            cache = key.CreateResource();

            if (this._initialized == false)
            {
                return cache;
            }

            cache.Refresh(this._resourcePackService.Value);
            return cache;
        }

        public IEnumerable<ResourceKey<T>> GetKeys<T>()
            where T : notnull
        {
            return ResourceKey<T>.GetAll();
        }
    }
}