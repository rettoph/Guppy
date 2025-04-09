using Guppy.Core.Files.Common;

namespace Guppy.Core.Assets.Common.Configuration
{
    public sealed class AssetPackCollectionConfiguration(List<AssetPackConfiguration> packs)
    {
        private readonly Dictionary<DirectoryPath, AssetPackConfiguration> _packs = packs.ToDictionary(x => x.EntryDirectory, x => x);

        public IEnumerable<AssetPackConfiguration> Packs => this._packs.Values;

        public AssetPackCollectionConfiguration() : this([])
        {
        }

        public AssetPackCollectionConfiguration Add(AssetPackConfiguration pack)
        {
            this._packs.Add(pack.EntryDirectory, pack);

            return this;
        }

        public AssetPackCollectionConfiguration AddRange(IEnumerable<AssetPackConfiguration> packs)
        {
            foreach (AssetPackConfiguration pack in packs)
            {
                this._packs.TryAdd(pack.EntryDirectory, pack);
            }

            return this;
        }
    }
}