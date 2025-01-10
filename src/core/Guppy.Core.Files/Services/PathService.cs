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
                DirectoryTypeEnum.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this._context.Company, this._context.Name, directory.Path),
                DirectoryTypeEnum.CurrentDirectory => Path.Combine(DirectoryHelper.GetEntryDirectory(), directory.Path),
                _ => directory.Path
            };

            return new DirectoryLocation(DirectoryTypeEnum.Source, path);
        }

        public DirectoryLocation GetSourceLocation(DirectoryTypeEnum type, string path) => this.GetSourceLocation(new DirectoryLocation(type, path));

        public FileLocation GetSourceLocation(FileLocation file) => new(this.GetSourceLocation(file.Directory), file.Name);

        public FileLocation GetSourceLocation(DirectoryTypeEnum type, string path, string name) => this.GetSourceLocation(new FileLocation(new DirectoryLocation(type, path), name));
    }
}