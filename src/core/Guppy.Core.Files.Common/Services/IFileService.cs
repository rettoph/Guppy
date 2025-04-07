namespace Guppy.Core.Files.Common.Services
{
    public interface IFileService
    {
        IFile Get(FilePath location, bool forceLoadFromDisk = false, bool createIfDoesNotExist = false);
        IFile<T> Get<T>(FilePath location, bool forceLoadFromDisk = false, bool createIfDoesNotExist = false);

        void Save<T>(IFile<T> file);
    }
}