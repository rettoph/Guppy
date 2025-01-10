using System.Runtime.InteropServices;
using Guppy.Core.Common;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Providers;
using Serilog;
using Serilog.Events;

namespace Guppy.Core.Services
{
    public class LoggerService : ILoggerService
    {
        private const string _sourceContextPropertyName = "SourceContext";

        private readonly ILogLevelService _logLevelService;
        private readonly Dictionary<string, ILogger> _loggers;
        private readonly Dictionary<Type, string> _contexts;

        public ILogger Base { get; }
        public ILogger Default { get; }

        public LoggerService(
            ILogLevelService logLevelService,
            IConfiguration<LoggerConfiguration> configuration,
            IEnumerable<LoggerContext> loggerContexts)
        {
            this._loggers = [];
            this._logLevelService = logLevelService;
            this.Base = configuration.Value
                .MinimumLevel.Is(LogEventLevel.Verbose)
                .CreateLogger();
            this.Default = new LoggerConfiguration()
                .Enrich.WithProperty(_sourceContextPropertyName, "Default")
                .MinimumLevel.Is(this._logLevelService.TryGetLogLevel(null) ?? throw new NotImplementedException())
                .WriteTo.Logger(this.Base)
                .CreateLogger();
            this._contexts = loggerContexts.ToDictionary(x => x.ServiceType, x => x.Context);
        }


        public ILogger GetOrCreate(Type? contextType)
        {
            if (contextType is null)
            {
                return this.Default;
            }

            string source = contextType.GetFormattedName();

            ref ILogger? logger = ref CollectionsMarshal.GetValueRefOrAddDefault(this._loggers, source, out bool exists);
            if (exists == true)
            {
                return logger!;
            }

            LogEventLevel? level;
            if (this._contexts.TryGetValue(contextType, out string? context) == true)
            {
                level = this._logLevelService.GetOrCreateLogLevel(context);
            }
            else
            {
                context = source;
                level = this._logLevelService.TryGetLogLevel(context);
            }

            if (level is null)
            {
                logger = this.Default.ForContext(_sourceContextPropertyName, source);
            }
            else
            {
                logger = new LoggerConfiguration()
                    .MinimumLevel.Is(level.Value)
                    .Enrich.WithProperty(_sourceContextPropertyName, source)
                    .WriteTo.Logger(this.Base)
                    .CreateLogger();
            }

            return logger ?? throw new NotImplementedException();
        }
    }
}