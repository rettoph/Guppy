using Guppy.Common.Attributes;
using Guppy.Enums;
using Guppy.Files;
using Guppy.Files.Enums;
using Guppy.Files.Services;
using Guppy.Resources.Configuration;
using Guppy.Resources.Constants;
using Guppy.Resources.Loaders;
using Guppy.Resources.ResourceTypes;
using Serilog;

namespace Guppy.Resources.Providers
{
    [Sequence<InitializeSequence>(InitializeSequence.PreInitialize)]
    internal sealed class ResourcePackProvider : GlobalComponent, IResourcePackProvider, IGlobalComponent
    {
        private readonly IFileService _files;
        private IDictionary<Guid, ResourcePack> _packs;
        private IFile<List<IFile<ResourcePackConfiguration>>> _registered;
        private readonly IResourceTypeProvider _resourceTypes;
        private readonly ILogger _logger;
        private readonly IPackLoader[] _loaders;

        public ResourcePackProvider(
            IFileService files,
            IEnumerable<ResourcePack> packs,
            IEnumerable<IPackLoader> loaders,
            IResourceTypeProvider resourceTypes,
            ILogger logger)
        {
            _files = files;
            _registered = _files.Get<List<IFile<ResourcePackConfiguration>>>(FileType.AppData, FilePaths.ResourcePacks);
            _resourceTypes = resourceTypes;
            _logger = logger;
            _loaders = loaders.ToArray();

            _packs = packs.ToDictionary(x => x.Id, x => x);
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            foreach (var loader in _loaders)
            {
                loader.Load(this);
            }

            foreach (IFile<ResourcePackConfiguration> configuration in _registered.Value)
            {
                this.Load(Path.GetDirectoryName(configuration.FullPath)!, configuration);
            }
        }

        public void Register(IFile<ResourcePackConfiguration> configuration)
        {
            if (_registered.Value.Any(x => x.Value.Id == configuration.Value.Id))
            {
                _logger.Warning("{ClassName}::{MethodName} - Duplicate registration of resource pack {Id}, {Name}", nameof(ResourcePackProvider), nameof(Register), configuration.Value.Id, configuration.Value.Name);
                return;
            }

            _registered.Value.Add(configuration);
            _files.Save(_registered);

            this.Load(Path.GetDirectoryName(configuration.FullPath)!, configuration);
        }

        public void Register(FileType type, string path)
        {
            this.Register(_files.Get<ResourcePackConfiguration>(type, path));
        }

        public IEnumerable<ResourcePack> GetAll()
        {
            return _packs.Values;
        }

        public ResourcePack GetById(Guid id)
        {
            return _packs[id];
        }

        private void Load(string directory, IFile<ResourcePackConfiguration> configuration)
        {
            ResourcePack pack = this.GetOrCreatePack(configuration);
            _logger.Information("{ClassName}::{MethodName} - Preparing to load resource pack {ResourcePackName}, {ResourcePackId}", nameof(ResourcePackProvider), nameof(Load), pack.Name, pack.Id);

            foreach ((string localization, string[] resourceFileNames) in configuration.Value.Import)
            {
                foreach (string resourceFileName in resourceFileNames)
                {
                    _logger.Information("{ClassName}::{MethodName} - Loading resource file {ResourceFile}, {Localization}", nameof(ResourcePackProvider), nameof(Load), resourceFileName, localization);

                    IFile<ResourceTypeValues[]> resourceTypeValuesFile = _files.Get<ResourceTypeValues[]>(FileType.Source, Path.Combine(directory, resourceFileName));
                    foreach (ResourceTypeValues resourceTypeValues in resourceTypeValuesFile.Value)
                    {
                        if (!_resourceTypes.TryGet(resourceTypeValues.Type, out IResourceType? resourceType))
                        {
                            _logger.Error("{ClassName}::{MethodName} - Unable to resolve resource type defined by {ResourceName}, unknown.", nameof(ResourcePackProvider), nameof(Load), resourceTypeValues.Type);
                            break;
                        }

                        foreach (var (name, json) in resourceTypeValues.Values)
                        {
                            if (!resourceType.TryResolve(pack, name, localization, json))
                            {
                                _logger.Error("{ClassName}::{MethodName} - Unable to resolve resource {ResourceName}", nameof(ResourcePackProvider), nameof(Load), name);
                            }
                        }
                    }
                }
            }
        }

        private ResourcePack GetOrCreatePack(IFile<ResourcePackConfiguration> configuration)
        {
            if (!_packs.TryGetValue(configuration.Value.Id, out var pack))
            {
                _packs[configuration.Value.Id] = pack = new ResourcePack(configuration);
            }

            return pack;
        }
    }
}
