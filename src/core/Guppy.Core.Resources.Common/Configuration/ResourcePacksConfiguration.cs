using Guppy.Core.Files.Common;

namespace Guppy.Core.Resources.Common.Configuration
{
    internal sealed class ResourcePacksConfiguration(List<ResourcePackConfiguration> packs)
    {
        private readonly Dictionary<DirectoryLocation, ResourcePackConfiguration> _packs = packs.ToDictionary(x => x.EntryDirectory, x => x);

        internal IEnumerable<ResourcePackConfiguration> Packs => _packs.Values;

        public ResourcePacksConfiguration() : this(new List<ResourcePackConfiguration>())
        {
        }

        public ResourcePacksConfiguration Add(ResourcePackConfiguration pack)
        {
            _packs.Add(pack.EntryDirectory, pack);

            return this;
        }

        public ResourcePacksConfiguration AddRange(IEnumerable<ResourcePackConfiguration> packs)
        {
            foreach (ResourcePackConfiguration pack in packs)
            {
                _packs.TryAdd(pack.EntryDirectory, pack);
            }

            return this;
        }
    }
}
