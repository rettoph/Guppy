using Guppy.Files.Enums;

namespace Guppy.Files.Services
{
    public interface IFileService
    {
        IFile Get(FileType type, string path, bool forceLoadFromDisk = false);
        IFile<T> Get<T>(FileType type, string path, bool forceLoadFromDisk = false);

        void Save<T>(IFile<T> file);
    }
}
