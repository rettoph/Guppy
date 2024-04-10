using Guppy.Files;
using Guppy.Resources.Configuration;
using Guppy.Resources.Constants;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Guppy.Resources
{
    public sealed class ResourcePack
    {
        private readonly IFile<ResourcePackEntryConfiguration> _entry;

        public Guid Id => _entry.Value.Id;
        public string Name => _entry.Value.Name;
        public readonly DirectoryLocation RootDirectory;

        private Dictionary<IResource, Dictionary<string, object>> _resources;

        internal ResourcePack(IFile<ResourcePackEntryConfiguration> entry, DirectoryLocation rootDirectory)
        {
            _entry = entry;
            this.RootDirectory = rootDirectory;

            _resources = new Dictionary<IResource, Dictionary<string, object>>();
        }

        public void Add<T>(Resource<T> resource, string localization, T value)
            where T : notnull
        {
            if (!_resources.TryGetValue(resource, out var values))
            {
                _resources[resource] = values = new();
            }

            ref object? localized = ref CollectionsMarshal.GetValueRefOrAddDefault(values, localization, out bool exists);
            localized ??= value;
        }

        public void Add<T>(Resource<T> resource, T value)
            where T : notnull
        {
            this.Add(resource, Localization.es_US, value);
        }

        public bool TryGet<T>(Resource<T> resource, string localization, [MaybeNullWhen(false)] out T value)
            where T : notnull
        {
            if (!_resources.TryGetValue(resource, out var values))
            {
                value = default;
                return false;
            }

            if (values.TryGetValue(localization, out object? cached))
            {
                value = (T)cached;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerable<Resource<T>> GetAll<T>()
            where T : notnull
        {
            return _resources.Keys.OfType<Resource<T>>();
        }
    }
}
