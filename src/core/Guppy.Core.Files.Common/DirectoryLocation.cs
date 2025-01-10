using Guppy.Core.Files.Common.Enums;

namespace Guppy.Core.Files.Common
{
    public readonly struct DirectoryLocation(DirectoryTypeEnum type, string path)
    {
        public readonly DirectoryTypeEnum Type = type;
        public readonly string Path = path;

        public static DirectoryLocation AppData(string? path = null)
        {
            return new(DirectoryTypeEnum.AppData, path ?? string.Empty);
        }

        public static DirectoryLocation CurrentDirectory(string? path = null)
        {
            return new(DirectoryTypeEnum.CurrentDirectory, path ?? string.Empty);
        }

        public static DirectoryLocation Source(string? path = null)
        {
            return new(DirectoryTypeEnum.Source, path ?? string.Empty);
        }

        public override readonly string ToString()
        {
            return $"({this.Type}):{this.Path}";
        }
    }
}