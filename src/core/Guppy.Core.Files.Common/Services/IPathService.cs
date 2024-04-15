using Guppy.Core.Files.Common.Enums;

namespace Guppy.Core.Files.Common.Services
{
    public interface IPathService
    {
        DirectoryLocation GetSourceLocation(DirectoryLocation directory);
        DirectoryLocation GetSourceLocation(DirectoryType type, string path);

        FileLocation GetSourceLocation(FileLocation file);
        FileLocation GetSourceLocation(DirectoryType type, string path, string name);
    }
}
