using System.Runtime.InteropServices;
using Guppy.Core.Common.Configurations;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Logging.Common.Services;

namespace Guppy.Core.Logging.Services
{
    internal sealed class LogLevelService(IFileService fileService) : ILogLevelService
    {
        private static readonly FileLocation _logLevelConfigurationFileLocation = FileLocation.AppData("logger.config.json");

        private readonly IFileService _fileService = fileService;
        private readonly IFile<LogLevelConfiguration> _configuration = fileService.Get<LogLevelConfiguration>(location: _logLevelConfigurationFileLocation, createIfDoesNotExist: true);

        public LogLevelEnum? TryGetLogLevel(Type? context)
        {
            if (context is null)
            {
                return this._configuration.Value.Default;
            }

            this._configuration.Value.Overrides.TryGetValue(context.GetFormattedName(), out var overrides);
            return overrides;
        }

        public LogLevelEnum GetLogLevel(Type? context)
        {
            if (context is null)
            {
                return this._configuration.Value.Default;
            }

            ref LogLevelEnum? level = ref CollectionsMarshal.GetValueRefOrAddDefault(this._configuration.Value.Overrides, context.GetFormattedName(), out bool exists);
            if (exists == false)
            {
                this._fileService.Save(this._configuration);
            }

            return level ?? this._configuration.Value.Default;
        }
    }
}