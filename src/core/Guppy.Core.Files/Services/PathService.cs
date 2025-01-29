using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Services;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Enums;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Files.Common.Services;

namespace Guppy.Core.Files.Services
{
    internal class PathService(IEnvironmentVariableService environmentVariableService) : IPathService
    {
        private readonly IEnvironmentVariableService _environmentVariableService = environmentVariableService;

        public DirectoryLocation GetSourceLocation(DirectoryLocation directory)
        {
            string path = directory.Type switch
            {
                DirectoryTypeEnum.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this._environmentVariableService.Get<GuppyVariables.Global.Company>().Value, this._environmentVariableService.Get<GuppyVariables.Global.Project>().Value, directory.Path),
                DirectoryTypeEnum.CurrentDirectory => Path.Combine(DirectoryHelper.GetEntryDirectory(), directory.Path),
                _ => directory.Path
            };

            return new DirectoryLocation(DirectoryTypeEnum.Source, path);
        }

        public DirectoryLocation GetSourceLocation(DirectoryTypeEnum type, string path)
        {
            return this.GetSourceLocation(new DirectoryLocation(type, path));
        }

        public FileLocation GetSourceLocation(FileLocation file)
        {
            return new(this.GetSourceLocation(file.Directory), file.Name);
        }

        public FileLocation GetSourceLocation(DirectoryTypeEnum type, string path, string name)
        {
            return this.GetSourceLocation(new FileLocation(new DirectoryLocation(type, path), name));
        }
    }
}