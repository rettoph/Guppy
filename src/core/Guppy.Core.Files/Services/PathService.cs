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

        public string GetFileSystemPath(DirectoryPath directory)
        {
            string fileSystemPath = directory.Type switch
            {
                DirectoryTypeEnum.AppData => Path.Combine([
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    this._environmentVariableService.Get<GuppyCoreVariables.Environment.Company>().Value,
                    this._environmentVariableService.Get<GuppyCoreVariables.Environment.Project>().Value,
                    directory.Path
                ]),
                DirectoryTypeEnum.CurrentDirectory => Path.Combine([
                    DirectoryHelper.GetEntryDirectory(),
                    directory.Path
                ]),
                _ => directory.Path
            };

            return fileSystemPath;
        }

        public string GetFileSystemPath(DirectoryTypeEnum type, string path)
        {
            return this.GetFileSystemPath(new DirectoryPath(type, path));
        }

        public string GetFileSystemPath(FilePath file)
        {
            return Path.Combine([
                this.GetFileSystemPath(file.DirectoryPath),
                file.FileName
            ]);
        }

        public string GetFileSystemPath(DirectoryTypeEnum type, string path, string name)
        {
            return this.GetFileSystemPath(new FilePath(new DirectoryPath(type, path), name));
        }
    }
}