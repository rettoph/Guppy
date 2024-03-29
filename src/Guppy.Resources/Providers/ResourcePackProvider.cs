﻿using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Enums;
using Guppy.Files;
using Guppy.Files.Services;
using Guppy.Resources.Configuration;
using Guppy.Resources.Constants;
using Guppy.Resources.ResourceTypes;
using Serilog;

namespace Guppy.Resources.Providers
{
    [Sequence<InitializeSequence>(InitializeSequence.PreInitialize)]
    internal sealed class ResourcePackProvider : GlobalComponent, IResourcePackProvider, IGlobalComponent
    {
        private readonly IFileService _files;
        private IDictionary<Guid, ResourcePack> _packs;
        private IFile<ResourcePacksConfiguration> _configuration;
        private readonly IResourceTypeProvider _resourceTypes;
        private readonly ILogger _logger;

        public ResourcePackProvider(
            IFileService files,
            IFiltered<ResourcePackConfiguration> packs,
            IResourceTypeProvider resourceTypes,
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
            foreach (ResourcePackConfiguration packConfiguration in _configuration.Value.Packs)
            {
                this.Load(packConfiguration);
            }
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
            _logger.Information("{ClassName}::{MethodName} - Preparing to load resource pack {ResourcePackName}, {ResourcePackId}", nameof(ResourcePackProvider), nameof(Load), pack.Name, pack.Id);

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
            _logger.Information("{ClassName}::{MethodName} - Loading resource file {ResourceFile}, {Localization}", nameof(ResourcePackProvider), nameof(ImportResourceFile), resourceFileName, localization);

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
