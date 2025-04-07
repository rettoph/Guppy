using Guppy.Core.Files.Common.Enums;

namespace Guppy.Core.Files.Common.Services
{
    public interface IPathService
    {
        DirectoryPath GetSourceLocation(DirectoryPath directory);
        DirectoryPath GetSourceLocation(DirectoryTypeEnum type, string path);

        FilePath GetSourceLocation(FilePath file);
        FilePath GetSourceLocation(DirectoryTypeEnum type, string path, string name);
    }
}