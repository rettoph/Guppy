using Guppy.Files.Enums;

namespace Guppy.Files.Providers
{
    public interface IFileTypePathProvider
    {
        string GetFullPath(FileType type, string path);
    }
}
