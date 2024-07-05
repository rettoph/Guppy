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

        public static DirectoryLocation AppData(string? path = null)
        {
            return new DirectoryLocation(DirectoryType.AppData, path ?? string.Empty);
        }

        public static DirectoryLocation CurrentDirectory(string? path = null)
        {
            return new DirectoryLocation(DirectoryType.CurrentDirectory, path ?? string.Empty);
        }

        public static DirectoryLocation Source(string? path = null)
        {
            return new DirectoryLocation(DirectoryType.Source, path ?? string.Empty);
        }

        public override string ToString()
        {
            return $"({this.Type}):{this.Path}";
        }
    }
}
