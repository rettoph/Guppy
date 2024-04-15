using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Constants;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Serilog;

namespace Guppy.Core.Resources.Services
{
    [Sequence<InitializeSequence>(InitializeSequence.PreInitialize)]
    internal sealed class ResourcePackService : GlobalComponent, IResourcePackService, IGlobalComponent
    {
        private bool _initialized;
        private readonly IFileService _files;
        private IDictionary<Guid, ResourcePack> _packs;
        private IFile<ResourcePacksConfiguration> _configuration;
        private readonly IResourceTypeService _resourceTypes;
        private readonly ILogger _logger;

        public ResourcePackService(
            IFileService files,
            IFiltered<ResourcePackConfiguration> packs,
            IResourceTypeService resourceTypes,
            ILogger logger)
        {
            _files = files;
            _configuration = _files.Get<ResourcePacksConfiguration>(new FileLocation(DirectoryLocation.AppData(string.Empty), FilePaths.ResourcePacksConfiguration));
            _resourceTypes = resourceTypes;
            _logger = logger;
            _packs = new Dictionary<Guid, ResourcePack>();

            _configuration.Value = _configuration.Value.AddRange(packs.Instances);
            _files.Save(_configuration);
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            this.Initialize();
        }

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            foreach (ResourcePackConfiguration packConfiguration in _configuration.Value.Packs)
            {
                this.Load(packConfiguration);
            }

            _initialized = true;
        }

        public IEnumerable<ResourcePack> GetAll()
        {
            return _packs.Values;
        }

        public ResourcePack GetById(Guid id)
        {
            return _packs[id];
        }

        private ResourcePack GetOrCreatePack(IFile<ResourcePackEntryConfiguration> entry)
        {
            if (!_packs.TryGetValue(entry.Value.Id, out var pack))
            {
                _packs[entry.Value.Id] = pack = new ResourcePack(
                    entry: entry,
                    rootDirectory: entry.Source.Directory);
            }

            return pack;
        }

        private void Load(ResourcePackConfiguration configuration)
        {
            IFile<ResourcePackEntryConfiguration> entry = _files.Get<ResourcePackEntryConfiguration>(new FileLocation(configuration.EntryDirectory, "pack.json"));
            DirectoryLocation directory = entry.Source.Directory;

            ResourcePack pack = this.GetOrCreatePack(entry);
            _logger.Information("{ClassName}::{MethodName} - Preparing to load resource pack {ResourcePackName}, {ResourcePackId}", nameof(ResourcePackService), nameof(Load), pack.Name, pack.Id);

            foreach ((string localization, string[] resourceFileNames) in entry.Value.Import)
            {
                foreach (string resourceFileName in resourceFileNames)
                {
                    this.ImportResourceFile(resourceFileName, pack, directory, localization);
                }
            }
        }

        private void ImportResourceFile(string resourceFileName, ResourcePack pack, DirectoryLocation directory, string localization)
        {
            _logger.Information("{ClassName}::{MethodName} - Loading resource file {ResourceFile}, {Localization}", nameof(ResourcePackService), nameof(ImportResourceFile), resourceFileName, localization);

            IFile<ResourceTypeValues[]> resourceTypeValuesFile = _files.Get<ResourceTypeValues[]>(new FileLocation(directory, resourceFileName));
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
