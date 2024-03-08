using Guppy.Common;
using Guppy.Files.Enums;
using Guppy.Files.Helpers;

namespace Guppy.Files.Providers
{
    internal class FilePathProvider : IFilePathProvider
    {
        private readonly IGuppyEnvironment _environment;

        public FilePathProvider(IGuppyEnvironment environment)
        {
            _environment = environment;
        }

        public string GetFullPath(FileLocation location)
        {
            return location.Type switch
            {
                FileType.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _environment.Company, _environment.Name, location.Path),
                FileType.CurrentDirectory => Path.Combine(DirectoryHelper.GetEntryDirectory(), location.Path),
                _ => location.Path
            };
        }

        public string GetFullPath(FileType type, string path)
        {
            return this.GetFullPath(new FileLocation(type, path));
        }
    }
}
