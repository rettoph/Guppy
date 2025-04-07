namespace Guppy.Core.Files.Common.Services
{
    public interface IFileService
    {
        IFile Get(FilePath path, bool forceLoadFromDisk = false, bool createIfDoesNotExist = false);
        IFile<T> Get<T>(FilePath path, bool forceLoadFromDisk = false, bool createIfDoesNotExist = false);

        void Save<T>(IFile<T> file);
    }
}