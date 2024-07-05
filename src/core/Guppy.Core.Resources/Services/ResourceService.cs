using Guppy.Core.Common.Services;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using System.Runtime.InteropServices;

namespace Guppy.Core.Resources.Services
{
    internal class ResourceService : IHostedService, IResourceService, IDisposable
    {
        private bool _initialized;
        private Lazy<IResourcePackService> _packs;

        private Dictionary<Guid, IResourceValue> _values;

        public ResourceService(Lazy<ISettingService> settings, Lazy<IResourcePackService> packs)
        {
            _packs = packs;
            _values = new Dictionary<Guid, IResourceValue>();
        }

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

            _packs.Value.Initialize();

            _initialized = true;

            this.Refresh();
        }

        public void Refresh()
        {
            if (_initialized == false)
            {
                return;
            }

            foreach (IResource resource in _packs.Value.GetDefinedResources())
            {
                this.CacheGetOrAddValues(resource).Refresh(_packs.Value);
            }

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

            cache.Refresh(_packs.Value);
            return cache;
        }

        public IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull
        {
            return Resource<T>.GetAll();
        }
    }
}
