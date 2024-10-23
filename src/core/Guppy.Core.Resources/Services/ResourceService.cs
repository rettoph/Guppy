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

        private readonly Dictionary<Guid, IResourceValue> _values = [];

        public void Dispose()
        {
            foreach (IResourceValue resourceValue in _values.Values)
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

            _logger.Value.Debug("{ClassName}::{MethodName} - Preparing to build resource value dictionary", nameof(ResourceService), nameof(Initialize));

            foreach (IResource resource in _resourcePackService.Value.GetDefinedResources())
            {
                this.CacheGetOrAddValues(resource).Refresh(_resourcePackService.Value);
            }

            _logger.Value.Debug("{ClassName}::{MethodName} - Done. Found ({Count}) resources", nameof(ResourceService), nameof(Initialize), _values.Count);
            foreach (IResourceValue value in _values.Values)
            {
                _logger.Value.Verbose("{ClassName}::{MethodName} - Resource = {Resource}, Type = {Type}, Value = {Value}, Count = {Count}", nameof(ResourceService), nameof(Initialize), value.Resource.Name, value.Resource.Type.GetFormattedName(), value.Value, value.All().Count());
            }

            _initialized = true;
        }

        public ResourceValue<T> GetValue<T>(Resource<T> resource)
            where T : notnull
        {
            return (ResourceValue<T>)this.CacheGetOrAddValues(resource);
        }

        public IEnumerable<ResourceValue<T>> GetValues<T>() where T : notnull
        {
            return Resource<T>.GetAll().Select(x => this.GetValue(x));
        }

        private IResourceValue CacheGetOrAddValues(IResource resource)
        {

            ref IResourceValue? cache = ref CollectionsMarshal.GetValueRefOrAddDefault(_values, resource.Id, out bool exists);
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

        public IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull
        {
            return Resource<T>.GetAll();
        }
    }
}
