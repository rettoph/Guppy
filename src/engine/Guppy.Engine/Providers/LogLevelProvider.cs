using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Guppy.Engine.Common.Configurations;
using Guppy.Engine.Common.Providers;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Runtime.InteropServices;

namespace Guppy.Engine.Providers
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

        public void Configure(LoggerConfiguration configuration)
        {
            configuration.MinimumLevel.ControlledBy(new LoggingLevelSwitch(_configuration.Value.Default));

            foreach ((string context, LogEventLevel? level) in _configuration.Value.Contexts)
            {
                configuration.MinimumLevel.Override(context, level ?? _configuration.Value.Default);
            }
        }

        public LogEventLevel Get(string? context = null, LogEventLevel? defaultLevel = null)
        {
            if (context is null)
            {
                return _configuration.Value.Default;
            }

            ref LogEventLevel? level = ref CollectionsMarshal.GetValueRefOrAddDefault(_configuration.Value.Contexts, context, out bool exists);
            if (exists == false)
            {
                level = defaultLevel;
                _fileService.Save(_configuration);
            }

            return level ?? _configuration.Value.Default;
        }
    }
}
