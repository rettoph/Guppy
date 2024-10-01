using Guppy.Core.Common.Contexts;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Enums;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Files.Common.Services;

namespace Guppy.Core.Files.Services
{
    internal class PathService(IGuppyContext context) : IPathService
    {
        private readonly IGuppyContext _context = context;

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
            return this.GetSourceLocation(new DirectoryLocation(type, path));
        }

        public FileLocation GetSourceLocation(FileLocation file)
        {
            return new FileLocation(this.GetSourceLocation(file.Directory), file.Name);
        }

        public FileLocation GetSourceLocation(DirectoryType type, string path, string name)
        {
            return this.GetSourceLocation(new FileLocation(new DirectoryLocation(type, path), name));
        }
    }
}
