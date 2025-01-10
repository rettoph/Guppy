using System.Runtime.InteropServices;
using Guppy.Core.Common.Configurations;
using Guppy.Core.Common.Providers;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Serilog.Events;

namespace Guppy.Core.Services
{
    internal sealed class LogLevelService(IFileService fileService) : ILogLevelService
    {
        private static readonly FileLocation _logLevelConfigurationFileLocation = FileLocation.AppData("logger.config.json");

        private readonly IFileService _fileService = fileService;
        private readonly IFile<LogLevelConfiguration> _configuration = fileService.Get<LogLevelConfiguration>(_logLevelConfigurationFileLocation);

        public LogEventLevel? TryGetLogLevel(string? context)
        {
            if (context is null)
            {
                return this._configuration.Value.Default;
            }

            this._configuration.Value.Overrides.TryGetValue(context, out var overrides);
            return overrides;
        }

        public LogEventLevel GetOrCreateLogLevel(string? context)
        {
            if (context is null)
            {
                return this._configuration.Value.Default;
            }

            ref LogEventLevel? level = ref CollectionsMarshal.GetValueRefOrAddDefault(this._configuration.Value.Overrides, context, out bool exists);
            if (exists == false)
            {
                this._fileService.Save(this._configuration);
            }

            return level ?? this._configuration.Value.Default;
        }
    }
}