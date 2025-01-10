using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Constants;

namespace Guppy.Core.Resources
{
    public sealed class ResourcePack : IResourcePack
    {
        public Guid Id { get; }
        public string Name { get; }
        public DirectoryLocation RootDirectory { get; }

        private readonly Dictionary<IResourceKey, Dictionary<string, object>> _values;

        internal ResourcePack(Guid id, string name, DirectoryLocation rootDirectory)
        {
            this.Id = id;
            this.Name = name;
            this.RootDirectory = rootDirectory;

            this._values = [];
        }

        public void Add<T>(ResourceKey<T> key, string localization, T value)
            where T : notnull
        {
            if (!this._values.TryGetValue(key, out var resolvers))
            {
                this._values[key] = resolvers = [];
            }

            ref object? localizedValue = ref CollectionsMarshal.GetValueRefOrAddDefault(resolvers, localization, out _);
            localizedValue ??= value;
        }

        public void Add<T>(ResourceKey<T> key, T value)
            where T : notnull
        {
            this.Add(key, Localization.en_US, value);
        }

        public bool TryGetDefinedValue<T>(ResourceKey<T> key, string localization, [MaybeNullWhen(false)] out T value)
            where T : notnull
        {
            if (this._values.TryGetValue(key, out var values) == false)
            {
                value = default;
                return false;
            }

            if (values.TryGetValue(localization, out object? uncastedValue))
            {
                value = (T)uncastedValue;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerable<ResourceKey<T>> GetAllDefinedResources<T>()
            where T : notnull
        {
            return this._values.Keys.OfType<ResourceKey<T>>();
        }

        public IEnumerable<IResourceKey> GetAllDefinedResources()
        {
            return this._values.Keys;
        }
    }
}