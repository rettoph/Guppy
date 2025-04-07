using Guppy.Core.Files.Common;

namespace Guppy.Core.Resources.Common.Configuration
{
    public class ResourcePackConfiguration
    {
        public DirectoryPath EntryDirectory { get; set; }
        public bool Enabled { get; set; } = true;
    }
}