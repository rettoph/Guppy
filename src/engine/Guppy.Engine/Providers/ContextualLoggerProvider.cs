using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Engine.Common.Attributes;
using Guppy.Engine.Common.Providers;
using Serilog;
using System.Reflection;

namespace Guppy.Engine.Providers
{
    public class ContextualLoggerProvider
    {
        private readonly ILogLevelProvider _logLevelService;
        private readonly ILogger _logger;

        public ContextualLoggerProvider(
            ILogLevelProvider logLevelService,
            IConfiguration<LoggerConfiguration> configuration)
        {
            _logLevelService = logLevelService;
            _logger = configuration.Value.CreateLogger();
        }

        public ILogger Get(Type type)
        {
            string name = type.HasCustomAttribute<LoggerContextAttribute>(true)
                ? type.GetCustomAttribute<LoggerContextAttribute>(true)!.Name
                : type.GetFormattedName();

            // Ensures the service name gets added to the logger config file
            _logLevelService.Get(name);

            return _logger.ForContext("SourceContext", name);
        }
    }
}
