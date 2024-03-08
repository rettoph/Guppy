using Guppy.Files.Enums;

namespace Guppy.Files
{
    public struct FileLocation
    {
        public readonly FileType Type;
        public readonly string Path;

        public FileLocation(FileType type, string path)
        {
            this.Type = type;
            this.Path = path;
        }
    }
}
