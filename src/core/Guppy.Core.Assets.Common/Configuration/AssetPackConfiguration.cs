using Guppy.Core.Files.Common;

namespace Guppy.Core.Assets.Common.Configuration
{
    public class AssetPackConfiguration
    {
        public DirectoryPath EntryDirectory { get; set; }
        public bool Enabled { get; set; } = true;
    }
}