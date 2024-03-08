using Guppy.Files.Enums;

namespace Guppy.Files.Services
{
    public interface IFileService
    {
        IFile Get(FileLocation location, bool forceLoadFromDisk = false);
        IFile Get(FileType type, string path, bool forceLoadFromDisk = false);

        IFile<T> Get<T>(FileType type, string path, bool forceLoadFromDisk = false);
        IFile<T> Get<T>(FileLocation location, bool forceLoadFromDisk = false);

        void Save<T>(IFile<T> file);
    }
}
