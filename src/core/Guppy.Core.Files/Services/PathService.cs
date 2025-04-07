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

        public DirectoryPath GetSourceLocation(DirectoryPath directory)
        {
            string path = directory.Type switch
            {
                DirectoryTypeEnum.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this._environmentVariableService.Get<GuppyCoreVariables.Environment.Company>().Value, this._environmentVariableService.Get<GuppyCoreVariables.Environment.Project>().Value, directory.Path),
                DirectoryTypeEnum.CurrentDirectory => Path.Combine(DirectoryHelper.GetEntryDirectory(), directory.Path),
                _ => directory.Path
            };

            return new DirectoryPath(DirectoryTypeEnum.Source, path);
        }

        public DirectoryPath GetSourceLocation(DirectoryTypeEnum type, string path)
        {
            return this.GetSourceLocation(new DirectoryPath(type, path));
        }

        public FilePath GetSourceLocation(FilePath file)
        {
            return new(this.GetSourceLocation(file.Directory), file.FileName);
        }

        public FilePath GetSourceLocation(DirectoryTypeEnum type, string path, string name)
        {
            return this.GetSourceLocation(new FilePath(new DirectoryPath(type, path), name));
        }
    }
}