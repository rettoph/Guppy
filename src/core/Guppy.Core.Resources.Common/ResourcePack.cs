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

        private readonly Dictionary<IResource, Dictionary<string, object>> _values;

        internal ResourcePack(IFile<ResourcePackEntryConfiguration> entry, DirectoryLocation rootDirectory)
        {
            _entry = entry;
            this.RootDirectory = rootDirectory;

            _values = [];
        }

        public void Add<T>(Resource<T> resource, string localization, T value)
            where T : notnull
        {
            if (!_values.TryGetValue(resource, out var resolvers))
            {
                _values[resource] = resolvers = [];
            }

            ref object? localizedValue = ref CollectionsMarshal.GetValueRefOrAddDefault(resolvers, localization, out bool exists);
            localizedValue ??= value;
        }

        public void Add<T>(Resource<T> resource, T value)
            where T : notnull
        {
            this.Add(resource, Localization.es_US, value);
        }

        public bool TryGetDefinedValue<T>(Resource<T> resource, string localization, [MaybeNullWhen(false)] out T value)
            where T : notnull
        {
            if (!_values.TryGetValue(resource, out var resolvers))
            {
                value = default;
                return false;
            }

            if (resolvers.TryGetValue(localization, out object? uncastedValue))
            {
                value = (T)uncastedValue;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerable<Resource<T>> GetAllDefinedResources<T>()
            where T : notnull
        {
            return _values.Keys.OfType<Resource<T>>();
        }

        public IEnumerable<IResource> GetAllDefinedResources()
        {
            return _values.Keys;
        }
    }
}
