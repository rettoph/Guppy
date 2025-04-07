using Guppy.Core.Files.Common.Enums;

namespace Guppy.Core.Files.Common.Services
{
    public interface IPathService
    {
        string GetFileSystemPath(DirectoryPath directory);
        string GetFileSystemPath(DirectoryTypeEnum type, string path);

        string GetFileSystemPath(FilePath file);
        string GetFileSystemPath(DirectoryTypeEnum type, string path, string name);
    }
}