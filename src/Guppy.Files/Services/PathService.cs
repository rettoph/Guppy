using Guppy.Common;
using Guppy.Files.Enums;
using Guppy.Files.Helpers;

namespace Guppy.Files.Services
{
    internal class PathService : IPathService
    {
        private readonly IGuppyEnvironment _environment;

        public PathService(IGuppyEnvironment environment)
        {
            _environment = environment;
        }

        public DirectoryLocation GetSourceLocation(DirectoryLocation directory)
        {
            string path = directory.Type switch
            {
                DirectoryType.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _environment.Company, _environment.Name, directory.Path),
                DirectoryType.CurrentDirectory => Path.Combine(DirectoryHelper.GetEntryDirectory(), directory.Path),
                _ => directory.Path
            };

            return new DirectoryLocation(DirectoryType.Source, path);
        }

        public DirectoryLocation GetSourceLocation(DirectoryType type, string path)
        {
            return GetSourceLocation(new DirectoryLocation(type, path));
        }

        public FileLocation GetSourceLocation(FileLocation file)
        {
            return new FileLocation(GetSourceLocation(file.Directory), file.Name);
        }

        public FileLocation GetSourceLocation(DirectoryType type, string path, string name)
        {
            return GetSourceLocation(new FileLocation(new DirectoryLocation(type, path), name));
        }
    }
}
