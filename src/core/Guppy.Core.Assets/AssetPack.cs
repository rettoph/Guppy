using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Guppy.Core.Files.Common;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Constants;

namespace Guppy.Core.Assets
{
    public sealed class AssetPack : IAssetPack
    {
        public Guid Id { get; }
        public string Name { get; }
        public DirectoryPath RootDirectory { get; }

        private readonly Dictionary<IAssetKey, Dictionary<string, object>> _values;

        internal AssetPack(Guid id, string name, DirectoryPath rootDirectory)
        {
            this.Id = id;
            this.Name = name;
            this.RootDirectory = rootDirectory;

            this._values = [];
        }

        public void Add<T>(AssetKey<T> key, string localization, T value)
            where T : notnull
        {
            if (!this._values.TryGetValue(key, out var resolvers))
            {
                this._values[key] = resolvers = [];
            }

            ref object? localizedValue = ref CollectionsMarshal.GetValueRefOrAddDefault(resolvers, localization, out _);
            localizedValue ??= value;
        }

        public void Add<T>(AssetKey<T> key, T value)
            where T : notnull
        {
            this.Add(key, Localization.en_US, value);
        }

        public bool TryGetDefinedValue<T>(AssetKey<T> key, string localization, [MaybeNullWhen(false)] out T value)
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

        public IEnumerable<AssetKey<T>> GetAllDefinedAssets<T>()
            where T : notnull
        {
            return this._values.Keys.OfType<AssetKey<T>>();
        }

        public IEnumerable<IAssetKey> GetAllDefinedAssets()
        {
            return this._values.Keys;
        }
    }
}