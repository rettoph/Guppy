using Guppy.Core.Files.Common.Enums;

namespace Guppy.Core.Files.Common
{
    public struct DirectoryLocation
    {
        public readonly DirectoryType Type;
        public readonly string Path;

        public DirectoryLocation(DirectoryType type, string path)
        {
            this.Type = type;
            this.Path = path;
        }

        public static DirectoryLocation AppData(string path)
        {
            return new DirectoryLocation(DirectoryType.AppData, path);
        }

        public static DirectoryLocation CurrentDirectory(string path)
        {
            return new DirectoryLocation(DirectoryType.CurrentDirectory, path);
        }

        public static DirectoryLocation Source(string path)
        {
            return new DirectoryLocation(DirectoryType.Source, path);
        }
    }
}
