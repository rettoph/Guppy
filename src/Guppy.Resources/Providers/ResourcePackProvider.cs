using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Enums;
using Guppy.Files;
using Guppy.Files.Enums;
using Guppy.Files.Services;
using Guppy.Resources.Configuration;
using Guppy.Resources.Extensions;
using Guppy.Resources.ResourceTypes;
using Serilog;

namespace Guppy.Resources.Providers
{
    [Sequence<InitializeSequence>(InitializeSequence.PreInitialize)]
    internal sealed class ResourcePackProvider : GlobalComponent, IResourcePackProvider, IGlobalComponent
    {
        private readonly IFileService _files;
        private IDictionary<Guid, ResourcePack> _packs;
        private IFile<ResourcePacksConfiguration> _packsConfiguration;
        private readonly IResourceTypeProvider _resourceTypes;
        private readonly ILogger _logger;

        public ResourcePackProvider(
            IFileService files,
            IEnumerable<ResourcePack> packs,
            IResourceTypeProvider resourceTypes,
            ILogger logger,
            IConfiguration<ResourcePacksConfiguration> packsConfiguration)
        {
            _files = files;
            _packsConfiguration = _files.GetResourcePacksConfiguration();
            _resourceTypes = resourceTypes;
            _logger = logger;

            _packs = packs.ToDictionary(x => x.Id, x => x);

            _packsConfiguration.Value = packsConfiguration.Value.Merge(_packsConfiguration.Value);
            _files.Save(_packsConfiguration);
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            foreach (FileLocation packConfigurationLocation in _packsConfiguration.Value.Packs)
            {
                this.Load(packConfigurationLocation);
            }
        }

        public void Register(FileLocation packConfiguration)
        {
            _packsConfiguration.Value.Add(packConfiguration);

            this.Load(packConfiguration);
        }

        public void Register(FileType type, string path)
        {
            this.Register(new FileLocation(type, path));
        }

        public IEnumerable<ResourcePack> GetAll()
        {
            return _packs.Values;
        }

        public ResourcePack GetById(Guid id)
        {
            return _packs[id];
        }

        private ResourcePack GetOrCreatePack(IFile<ResourcePackConfiguration> configuration)
        {
            if (!_packs.TryGetValue(configuration.Value.Id, out var pack))
            {
                _packs[configuration.Value.Id] = pack = new ResourcePack(configuration);
            }

            return pack;
        }

        private void Load(FileLocation location)
        {
            IFile<ResourcePackConfiguration> configuration = _files.Get<ResourcePackConfiguration>(location);
            string directory = Path.GetDirectoryName(configuration.FullPath) ?? throw new NotImplementedException();

            ResourcePack pack = this.GetOrCreatePack(configuration);
            _logger.Information("{ClassName}::{MethodName} - Preparing to load resource pack {ResourcePackName}, {ResourcePackId}", nameof(ResourcePackProvider), nameof(Load), pack.Name, pack.Id);

            foreach ((string localization, string[] resourceFileNames) in configuration.Value.Import)
            {
                foreach (string resourceFileName in resourceFileNames)
                {
                    this.ImportResourceFile(resourceFileName, pack, directory, localization);
                }
            }
        }

        private void ImportResourceFile(string resourceFileName, ResourcePack pack, string directory, string localization)
        {
            _logger.Information("{ClassName}::{MethodName} - Loading resource file {ResourceFile}, {Localization}", nameof(ResourcePackProvider), nameof(ImportResourceFile), resourceFileName, localization);

            IFile<ResourceTypeValues[]> resourceTypeValuesFile = _files.Get<ResourceTypeValues[]>(FileType.Source, Path.Combine(directory, resourceFileName));
            foreach (ResourceTypeValues resourceTypeValues in resourceTypeValuesFile.Value)
            {
                this.ResolveResourceTypeValues(resourceTypeValues, pack, localization);
            }
        }

        private void ResolveResourceTypeValues(ResourceTypeValues resourceTypeValues, ResourcePack pack, string localization)
        {
            if (_resourceTypes.TryGet(resourceTypeValues.Type, out IResourceType? resourceType) == false)
            {
                _logger.Error("{ClassName}::{MethodName} - Unable to resolve resource type defined by {ResourceName}, unknown.", nameof(ResourcePackProvider), nameof(ResolveResourceTypeValues), resourceTypeValues.Type);
                return;
            }

            foreach (var (name, json) in resourceTypeValues.Values)
            {
                if (resourceType.TryResolve(pack, name, localization, json) == false)
                {
                    _logger.Error("{ClassName}::{MethodName} - Unable to resolve resource {ResourceName}", nameof(ResourcePackProvider), nameof(ResolveResourceTypeValues), name);
                    continue;
                }
            }
        }
    }
}
