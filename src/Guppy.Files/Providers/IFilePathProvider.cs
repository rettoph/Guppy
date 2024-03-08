using Guppy.Files.Enums;

namespace Guppy.Files.Providers
{
    public interface IFilePathProvider
    {
        string GetFullPath(FileLocation location);
        string GetFullPath(FileType type, string path);
    }
}
