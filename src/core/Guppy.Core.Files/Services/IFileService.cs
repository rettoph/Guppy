namespace Guppy.Core.Files.Services
{
    public interface IFileService
    {
        IFile Get(FileLocation location, bool forceLoadFromDisk = false);
        IFile<T> Get<T>(FileLocation location, bool forceLoadFromDisk = false);

        void Save<T>(IFile<T> file);
    }
}
