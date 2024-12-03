using Guppy.Core.Files.Common;

namespace Guppy.Core.Resources.Common.Configuration
{
    public sealed class ResourcePackCollectionConfiguration(List<ResourcePackConfiguration> packs)
    {
        private readonly Dictionary<DirectoryLocation, ResourcePackConfiguration> _packs = packs.ToDictionary(x => x.EntryDirectory, x => x);

        public IEnumerable<ResourcePackConfiguration> Packs => _packs.Values;

        public ResourcePackCollectionConfiguration() : this([])
        {
        }

        public ResourcePackCollectionConfiguration Add(ResourcePackConfiguration pack)
        {
            _packs.Add(pack.EntryDirectory, pack);

            return this;
        }

        public ResourcePackCollectionConfiguration AddRange(IEnumerable<ResourcePackConfiguration> packs)
        {
            foreach (ResourcePackConfiguration pack in packs)
            {
                _packs.TryAdd(pack.EntryDirectory, pack);
            }

            return this;
        }
    }
}
