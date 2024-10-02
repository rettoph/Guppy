using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common.Configuration;
using Guppy.Core.Resources.Common.Constants;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Guppy.Core.Resources.Common
{
    public sealed class ResourcePack
    {
        private readonly IFile<ResourcePackEntryConfiguration> _entry;

        public Guid Id => _entry.Value.Id;
        public string Name => _entry.Value.Name;
        public readonly DirectoryLocation RootDirectory;

        private readonly Dictionary<IResource, Dictionary<string, ResourceResolver>> _resolvers;

        internal ResourcePack(IFile<ResourcePackEntryConfiguration> entry, DirectoryLocation rootDirectory)
        {
            _entry = entry;
            this.RootDirectory = rootDirectory;

            _resolvers = [];
        }

        public void Add<T>(Resource<T> resource, string localization, ResourceResolver<T> resolver)
            where T : notnull
        {
            if (!_resolvers.TryGetValue(resource, out var resolvers))
            {
                _resolvers[resource] = resolvers = [];
            }

            ref ResourceResolver? localizedResolver = ref CollectionsMarshal.GetValueRefOrAddDefault(resolvers, localization, out bool exists);
            localizedResolver ??= resolver;
        }

        public void Add<T>(Resource<T> resource, ResourceResolver<T> resolver)
            where T : notnull
        {
            this.Add(resource, Localization.es_US, resolver);
        }

        public bool TryGetDefinedValue<T>(Resource<T> resource, string localization, [MaybeNullWhen(false)] out T value)
            where T : notnull
        {
            if (!_resolvers.TryGetValue(resource, out var resolvers))
            {
                value = default;
                return false;
            }

            if (resolvers.TryGetValue(localization, out ResourceResolver? uncastedLocalizedResolver))
            {
                ResourceResolver<T> castedLocalizedResolver = (ResourceResolver<T>)uncastedLocalizedResolver;
                value = castedLocalizedResolver.Value;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerable<Resource<T>> GetAllDefinedResources<T>()
            where T : notnull
        {
            return _resolvers.Keys.OfType<Resource<T>>();
        }

        public IEnumerable<IResource> GetAllDefinedResources()
        {
            return _resolvers.Keys;
        }
    }
}
