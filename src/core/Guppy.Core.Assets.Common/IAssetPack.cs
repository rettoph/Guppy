using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Files.Common;

namespace Guppy.Core.Assets.Common
{
    public interface IAssetPack
    {
        Guid Id { get; }
        string Name { get; }
        DirectoryPath RootDirectory { get; }

        public void Add<T>(AssetKey<T> key, string localization, T value)
            where T : notnull;

        public void Add<T>(AssetKey<T> key, T value)
            where T : notnull;

        public bool TryGetDefinedValue<T>(AssetKey<T> key, string localization, [MaybeNullWhen(false)] out T value)
            where T : notnull;

        public IEnumerable<AssetKey<T>> GetAllDefinedAssets<T>()
            where T : notnull;

        public IEnumerable<IAssetKey> GetAllDefinedAssets();
    }
}