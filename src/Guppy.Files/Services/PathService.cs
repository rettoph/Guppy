using Guppy.Common.Contexts;
using Guppy.Files.Enums;
using Guppy.Files.Helpers;

namespace Guppy.Files.Services
{
    internal class PathService : IPathService
    {
        private readonly IGuppyContext _context;

        public PathService(IGuppyContext context)
        {
            _context = context;
        }

        public DirectoryLocation GetSourceLocation(DirectoryLocation directory)
        {
            string path = directory.Type switch
            {
                DirectoryType.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _context.Company, _context.Name, directory.Path),
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
