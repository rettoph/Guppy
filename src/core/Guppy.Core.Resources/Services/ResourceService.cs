using Guppy.Core.Common.Services;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Constants;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Core.Resources.Services
{
    internal class ResourceService : IHostedService, IResourceService, IDisposable
    {
        private bool _initialized;
        private ISettingService _settings;
        private IResourcePackService _packs;

        public ResourceService(ISettingService settings, IResourcePackService packs)
        {
            _settings = settings;
            _packs = packs;

            StaticCollection<IResource>.OnAdded += this.HandleResourceAdded;
        }

        public void Dispose()
        {
            StaticCollection<IResource>.OnAdded -= this.HandleResourceAdded;

            StaticCollection<IResource>.Clear(true);
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

            _settings.Initialize();
            _packs.Initialize();

            foreach (IResource resource in StaticCollection<IResource>.GetAll())
            {
                resource.Refresh();
            }

            _initialized = true;
        }

        public void RefreshAll()
        {
            throw new NotImplementedException();
        }

        public T GetLatestValue<T>(Resource<T> resource)
            where T : notnull
        {
            // TODO: Load default somehow
            List<T> valuesToCache = new List<T>();
            foreach (ResourcePack pack in _packs.GetAll())
            {
                if (pack.TryGet(resource, Settings.Localization.DefaultValue, out T? packValue))
                {
                    valuesToCache.Add(packValue);
                }
            }

            if (Settings.Localization.Value != Settings.Localization.DefaultValue)
            {
                foreach (ResourcePack pack in _packs.GetAll())
                {
                    if (pack.TryGet(resource, Settings.Localization.Value, out T? packValue))
                    {
                        valuesToCache.Add(packValue);
                    }
                }
            }

            return valuesToCache.LastOrDefault() ?? throw new NotImplementedException();
        }

        private void HandleResourceAdded(object? sender, IResource resource)
        {
            if (_initialized == false)
            {
                return;
            }

            resource.Refresh();
        }

        public IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull
        {
            return Resource<T>.GetAll();
        }
    }
}
