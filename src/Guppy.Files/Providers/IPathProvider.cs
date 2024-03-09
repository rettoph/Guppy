using Guppy.Files.Enums;

namespace Guppy.Files.Providers
{
    public interface IPathProvider
    {
        DirectoryLocation GetSourceLocation(DirectoryLocation directory);
        DirectoryLocation GetSourceLocation(DirectoryType type, string path);

        FileLocation GetSourceLocation(FileLocation file);
        FileLocation GetSourceLocation(DirectoryType type, string path, string name);
    }
}
