using Guppy.Resources.Configuration;
using Guppy.Resources.Constants;
using Guppy.Resources.Loaders;
using Guppy.Resources.Serialization.Json;
using Guppy.Files;
using Guppy.Files.Services;
using Guppy.Files.Enums;
using Guppy.Resources.Serialization.Resources;
using Serilog;
using System.Xml.Linq;

namespace Guppy.Resources.Providers
{
    internal sealed class ResourcePackProvider : IResourcePackProvider
    {
        private readonly IFileService _files;
        private IDictionary<Guid, ResourcePack> _packs;
        private IFile<List<IFile<ResourcePackConfiguration>>> _registered;
        private readonly Lazy<IResourceProvider> _resources;
        private readonly Dictionary<Type, IResourceTypeResolver> _resolvers;
        private readonly ILogger _logger;

        public ResourcePackProvider(
            IFileService options,
            IEnumerable<ResourcePack> packs,
            IEnumerable<IPackLoader> loaders,
            Lazy<IResourceProvider> resources,
            IEnumerable<IResourceTypeResolver> resolvers,
            ILogger logger)
        {
            _files = options;
            _registered = options.Get<List<IFile<ResourcePackConfiguration>>>(FileType.AppData, FilePaths.Packs);
            _resources = resources;
            _resolvers = resolvers.ToDictionary(x => x.Type, x => x);
            _logger = logger;

            _packs = packs.ToDictionary(x => x.Id, x => x);

            foreach(var loader in loaders)
            {
                loader.Load(this);
            }

            foreach(IFile<ResourcePackConfiguration> configuration in _registered.Value)
            {
                this.Load(Path.GetDirectoryName(configuration.FullPath)!, configuration);
            }
        }

        public void Register(IFile<ResourcePackConfiguration> configuration)
        {
            if(_registered.Value.Any(x => x.Value.Id == configuration.Value.Id))
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
            _logger.Verbose("{ClassName}::{MethodName} - Preparing to load resource pack {ResourcePackName}, {ResourcePackId}", nameof(ResourcePackProvider), nameof(Load), pack.Name, pack.Id);

            foreach ((string localization, string[] resourceFiles) in configuration.Value.Import)
            {
                foreach(string resourceFile in resourceFiles)
                {
                    Dictionary<string, RawResourceValues> rawResourceValues = _files.Get<Dictionary<string, RawResourceValues>>(
                        FileType.Source,
                        Path.Combine(directory, resourceFile)).Value;

                    _logger.Verbose("{ClassName}::{MethodName} - Loading resource file {ResourceFile}, {Localization}", nameof(ResourcePackProvider), nameof(Load), resourceFile, localization);

                    foreach ((string name, RawResourceValues values) in rawResourceValues)
                    {
                        if (!_resources.Value.TryGetResourceByName(name, out Resource? resource))
                        {
                            _logger.Warning("{ClassName}::{MethodName} - Unable to resolve resource {ResourceName}, unknown.", nameof(ResourcePackProvider), nameof(Load), name);
                            continue;
                        }

                        if(!_resolvers.TryGetValue(resource.Type, out IResourceTypeResolver? resolver))
                        {
                            _logger.Warning("{ClassName}::{MethodName} - Unknown resource type {Type} when attempting to load {Resource}", nameof(ResourcePackProvider), nameof(Load), resource.Type, name);
                            continue;
                        }

                        foreach(string value in values)
                        {
                            if (!resolver.TryResolve(pack, resource, localization, value))
                            {
                                _logger.Warning("{ClassName}::{MethodName} - Unable to resolve resource {ResourceName}, with value {Value}", nameof(ResourcePackProvider), nameof(Load), name, value);
                                continue;
                            }

                            _logger.Verbose("{ClassName}::{MethodName} - Successfully loaded resource {ResourceName} with value {Value}", nameof(ResourcePackProvider), nameof(Load), name, value);
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
