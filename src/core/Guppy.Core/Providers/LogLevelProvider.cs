using Guppy.Core.Common.Configurations;
using Guppy.Core.Common.Providers;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Serilog.Events;
using System.Runtime.InteropServices;

namespace Guppy.Core.Providers
{
    internal sealed class LogLevelProvider : ILogLevelProvider
    {
        private static readonly FileLocation LogLevelConfigurationFileLocation = FileLocation.AppData("loglevel.config.json");

        private readonly IFileService _fileService;
        private readonly IFile<LogLevelConfiguration> _configuration;

        public LogLevelProvider(IFileService fileService)
        {
            _fileService = fileService;
            _configuration = fileService.Get<LogLevelConfiguration>(LogLevelConfigurationFileLocation);
        }

        public LogEventLevel? Get(string? context)
        {
            if (context is null)
            {
                return _configuration.Value.Default;
            }

            ref LogEventLevel? level = ref CollectionsMarshal.GetValueRefOrAddDefault(_configuration.Value.Contexts, context, out bool exists);
            if (exists == false)
            {
                _fileService.Save(_configuration);
            }

            return level;
        }
    }
}
