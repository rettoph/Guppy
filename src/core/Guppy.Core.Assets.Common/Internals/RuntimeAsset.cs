using Guppy.Core.Assets.Common.Interfaces;

namespace Guppy.Core.Assets.Common.Internals
{
    internal class RuntimeAsset<T>(AssetKey<T> key, T value, string localization) : IRuntimeAsset
        where T : notnull
    {
        public AssetKey<T> Key { get; } = key;

        public T Value { get; } = value;

        public string Localization { get; } = localization;

        public void AddToPack(IAssetPack resourcePack)
        {
            resourcePack.Add(this.Key, this.Localization, this.Value);
        }
    }
}