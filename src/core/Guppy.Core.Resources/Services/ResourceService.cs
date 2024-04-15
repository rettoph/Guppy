using Guppy.Engine;
using Guppy.Engine.Common.Attributes;
using Guppy.Engine.Common.Utilities;
using Guppy.Engine.Enums;
using Guppy.Core.Resources.Constants;

namespace Guppy.Core.Resources.Services
{
    [Sequence<InitializeSequence>(InitializeSequence.PreInitialize)]
    internal class ResourceService : GlobalComponent, IResourceService, IDisposable
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

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            this.Initialize();
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
                resource.Initialize(this);
            }

            _initialized = true;
        }

        public void RefreshAll()
        {
            throw new NotImplementedException();
        }

        internal T GetPackValue<T>(Resource<T> resource)
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
            if (this.Ready == false)
            {
                return;
            }

            resource.Initialize(this);
        }

        public IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull
        {
            return Resource<T>.GetAll();
        }
    }
}
