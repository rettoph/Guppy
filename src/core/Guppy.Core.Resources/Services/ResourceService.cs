using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Services;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Serilog;
using System.Runtime.InteropServices;

namespace Guppy.Core.Resources.Services
{
    internal class ResourceService(Lazy<IResourcePackService> packs, Lazy<ILogger> logger) : IHostedService, IResourceService, IDisposable
    {
        private bool _initialized;
        private readonly Lazy<IResourcePackService> _resourcePackService = packs;
        private readonly Lazy<ILogger> _logger = logger;

        private readonly Dictionary<Guid, IResource> _values = [];

        public void Dispose()
        {
            foreach (IResource resourceValue in _values.Values)
            {
                resourceValue.Dispose();
            }

            _values.Clear();
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
            if (_initialized)
            {
                return;
            }

            _resourcePackService.Value.Initialize();

            _logger.Value.Debug("Preparing to build resource value dictionary");

            foreach (IResourceKey resourceKey in _resourcePackService.Value.GetDefinedResources())
            {
                this.CacheGetOrAddValues(resourceKey).Refresh(_resourcePackService.Value);
            }

            _logger.Value.Debug("Done. Found ({Count}) resources", _values.Count);
            foreach (IResource value in _values.Values)
            {
                _logger.Value.Verbose("Resource = {Resource}, Type = {Type}, Value = {Value}, Count = {Count}", value.Key.Name, value.Key.Type.GetFormattedName(), value.Value, value.All().Count());
            }

            _initialized = true;
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

        private IResource CacheGetOrAddValues(IResourceKey resource)
        {

            ref IResource? cache = ref CollectionsMarshal.GetValueRefOrAddDefault(_values, resource.Id, out bool exists);
            if (exists == true)
            {
                return cache!;
            }

            cache = resource.CreateValue();

            if (_initialized == false)
            {
                return cache;
            }

            cache.Refresh(_resourcePackService.Value);
            return cache;
        }

        public IEnumerable<ResourceKey<T>> GetKeys<T>()
            where T : notnull
        {
            return ResourceKey<T>.GetAll();
        }
    }
}
