using Guppy.Files;
using Guppy.Files.Enums;

namespace Guppy.Resources.Configuration
{
    public sealed class ResourcePacksConfiguration
    {
        private readonly HashSet<FileLocation> _packs;

        internal IEnumerable<FileLocation> Packs => _packs;

        public ResourcePacksConfiguration() : this(new List<FileLocation>())
        {
        }
        public ResourcePacksConfiguration(List<FileLocation> packs)
        {
            _packs = new HashSet<FileLocation>(packs);
        }

        public ResourcePacksConfiguration Add(FileLocation location)
        {
            _packs.Add(location);

            return this;
        }

        public ResourcePacksConfiguration Add(FileType type, string path)
        {
            return this.Add(new FileLocation(type, path));
        }

        public ResourcePacksConfiguration Merge(ResourcePacksConfiguration configuration)
        {
            foreach (FileLocation pack in configuration.Packs)
            {
                _packs.Add(pack);
            }

            return this;
        }
    }
}
