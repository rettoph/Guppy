using Guppy.Core.Common.Configurations;
using Guppy.Core.Common.Providers;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Serilog.Events;
using System.Runtime.InteropServices;

namespace Guppy.Core.Providers
{
    internal sealed class LoggerConfigurationProvider : ILoggerConfigurationProvider
    {
        private static readonly FileLocation LogLevelConfigurationFileLocation = FileLocation.AppData("logger.config.json");

        private readonly IFileService _fileService;
        private readonly IFile<LogLevelConfiguration> _configuration;

        public LoggerConfigurationProvider(IFileService fileService)
        {
            _fileService = fileService;
            _configuration = fileService.Get<LogLevelConfiguration>(LogLevelConfigurationFileLocation);
        }

        public LogEventLevel? TryGetLogLevel(string? context)
        {
            if (context is null)
            {
                return _configuration.Value.Default;
            }

            _configuration.Value.Overrides.TryGetValue(context, out var overrides);
            return overrides;
        }

        public LogEventLevel GetOrCreateLogLevel(string? context)
        {
            if (context is null)
            {
                return _configuration.Value.Default;
            }

            ref LogEventLevel? level = ref CollectionsMarshal.GetValueRefOrAddDefault(_configuration.Value.Overrides, context, out bool exists);
            if (exists == false)
            {
                _fileService.Save(_configuration);
            }

            return level ?? _configuration.Value.Default;
        }
    }
}
