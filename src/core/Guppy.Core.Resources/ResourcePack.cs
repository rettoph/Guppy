using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Constants;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Guppy.Core.Resources
{
    public sealed class ResourcePack : IResourcePack
    {
        public Guid Id { get; }
        public string Name { get; }
        public DirectoryLocation RootDirectory { get; }

        private readonly Dictionary<IResource, Dictionary<string, object>> _values;

        internal ResourcePack(Guid id, string name, DirectoryLocation rootDirectory)
        {
            this.Id = id;
            this.Name = name;
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
