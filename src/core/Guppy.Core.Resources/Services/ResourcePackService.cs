using Guppy.Core.Common;
using Guppy.Core.Common.Services;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Constants;
using Guppy.Core.Resources.Common.Interfaces;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Core.Resources.Common.Services;
using Serilog;

namespace Guppy.Core.Resources.Services
{
    internal sealed class ResourcePackService : IHostedService, IResourcePackService
    {
        private bool _initialized;
        private readonly IFileService _files;
        private readonly IDictionary<Guid, ResourcePack> _packs;
        private readonly IFile<ResourcePacksConfiguration> _configuration;
        private readonly IResourceTypeService _resourceTypes;
        private readonly ILogger _logger;
        private readonly Lazy<ISettingService> _settings;
        private readonly ResourcePack _runtimeResourcePack;

        private SettingValue<string> _localization;

        public ResourcePackService(
            IFileService files,
            IFiltered<ResourcePackConfiguration> packs,
            IFiltered<IRuntimeResourceValue> runtimeResourceValues,
            IResourceTypeService resourceTypes,
            Lazy<ISettingService> settings,
            ILogger logger)
        {
            _files = files;
            _configuration = _files.Get<ResourcePacksConfiguration>(new FileLocation(DirectoryLocation.AppData(string.Empty), FilePaths.ResourcePacksConfiguration));
            _resourceTypes = resourceTypes;
            _logger = logger;
            _packs = new Dictionary<Guid, ResourcePack>();
            _settings = settings;
            _runtimeResourcePack = new ResourcePack(Guid.Empty, "Runtime Resources", DirectoryLocation.CurrentDirectory());

            _configuration.Value = _configuration.Value.AddRange(packs);
            _files.Save(_configuration);

            foreach (IRuntimeResourceValue runtimeResourceValue in runtimeResourceValues)
            {
                runtimeResourceValue.AddToPack(_runtimeResourcePack);
            }
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

            _settings.Value.Initialize();
            _localization = _settings.Value.GetValue(Settings.Localization);

            _logger.Debug("{ClassName}::{MethodName} - Preparing to import resource packs", nameof(ResourcePackService), nameof(Initialize));

            foreach (ResourcePackConfiguration packConfiguration in _configuration.Value.Packs)
            {
                this.Load(packConfiguration);
            }

            // Add the runtime resource pack last
            _packs.Add(_runtimeResourcePack.Id, _runtimeResourcePack);
            _initialized = true;

            _logger.Debug("{ClassName}::{MethodName} - Done. Imported ({Count}) resource packs", nameof(ResourcePackService), nameof(Initialize), _packs.Count);
        }

        public IEnumerable<IResourcePack> GetAll()
        {
            return _packs.Values;
        }

        public IResourcePack GetById(Guid id)
        {
            return _packs[id];
        }

        public IEnumerable<IResource> GetDefinedResources()
        {
            return this.GetAll().SelectMany(x => x.GetAllDefinedResources()).Distinct();
        }

        public IEnumerable<T> GetDefinedValues<T>(Resource<T> resource)
            where T : notnull
        {
            foreach (ResourcePack pack in this.GetAll())
            {
                if (pack.TryGetDefinedValue(resource, Settings.Localization.DefaultValue, out T? packValue))
                {
                    yield return packValue;
                }
            }

            if (_localization != Settings.Localization.DefaultValue)
            {
                foreach (ResourcePack pack in this.GetAll())
                {
                    if (pack.TryGetDefinedValue(resource, _localization, out T? packValue))
                    {
                        yield return packValue;
                    }
                }
            }
        }

        private ResourcePack GetOrCreatePack(IFile<ResourcePackEntryConfiguration> entry)
        {
            if (!_packs.TryGetValue(entry.Value.Id, out var pack))
            {
                _packs[entry.Value.Id] = pack = new ResourcePack(
                    id: entry.Value.Id,
                    name: entry.Value.Name,
                    rootDirectory: entry.Source.Directory);
            }

            return pack;
        }

        private void Load(ResourcePackConfiguration configuration)
        {
            FileLocation entryLocation = new(configuration.EntryDirectory, "pack.json");
            IFile<ResourcePackEntryConfiguration> entry = _files.Get<ResourcePackEntryConfiguration>(entryLocation);
            DirectoryLocation directory = entry.Source.Directory;

            ResourcePack pack = this.GetOrCreatePack(entry);
            _logger.Debug("{ClassName}::{MethodName} - Preparing to load resource pack {ResourcePackName}, {ResourcePackId} resources", nameof(ResourcePackService), nameof(Load), pack.Name, pack.Id);

            foreach ((string localization, string[] resourceFileNames) in entry.Value.Import)
            {
                foreach (string resourceFileName in resourceFileNames)
                {
                    this.ImportResourceFile(resourceFileName, pack, directory, localization);
                }
            }

            _logger.Debug("{ClassName}::{MethodName} - Done. Loaded ({Count}) resources", nameof(ResourcePackService), nameof(Load), pack.GetAllDefinedResources().Count());
        }

        private void ImportResourceFile(string resourceFileName, ResourcePack pack, DirectoryLocation directory, string localization)
        {
            FileLocation resourceFileLocation = new(directory, resourceFileName);
            _logger.Verbose("{ClassName}::{MethodName} - Loading resource file {ResourceFile}, {Localization}", nameof(ResourcePackService), nameof(ImportResourceFile), resourceFileLocation, localization);

            IFile<ResourceTypeValues[]> resourceTypeValuesFile = _files.Get<ResourceTypeValues[]>(resourceFileLocation);
            foreach (ResourceTypeValues resourceTypeValues in resourceTypeValuesFile.Value)
            {
                this.ResolveResourceTypeValues(resourceTypeValues, pack, localization);
            }
        }

        private void ResolveResourceTypeValues(ResourceTypeValues resourceTypeValues, ResourcePack pack, string localization)
        {
            if (_resourceTypes.TryGet(resourceTypeValues.Type, out IResourceType? resourceType) == false)
            {
                _logger.Error("{ClassName}::{MethodName} - Unable to resolve resource type defined by {ResourceName}, unknown.", nameof(ResourcePackService), nameof(ResolveResourceTypeValues), resourceTypeValues.Type);
                return;
            }

            foreach (var (name, json) in resourceTypeValues.Values)
            {
                if (resourceType.TryResolve(pack, name, localization, json) == false)
                {
                    _logger.Error("{ClassName}::{MethodName} - Unable to resolve resource {ResourceName}", nameof(ResourcePackService), nameof(ResolveResourceTypeValues), name);
                    continue;
                }
            }
        }
    }
}
