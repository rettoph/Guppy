using Autofac;
using Autofac.Core.Resolving.Pipeline;
using Guppy.Core.Logging.Common;
using Guppy.Core.Logging.Common.Services;

namespace Guppy.Core.Logging.Middleware
{
    internal sealed class LoggingMiddleware(LoggerParameterContext loggerParameterContext) : IResolveMiddleware
    {
        private readonly LoggerParameterContext _loggerParameterContext = loggerParameterContext;

        public PipelinePhase Phase => PipelinePhase.ParameterSelection;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            ILoggerService loggerService = context.Resolve<ILoggerService>();
            ILogger logger = loggerService.GetLogger(this._loggerParameterContext.ContextType);

            TypedParameter parameter = new TypedParameter(this._loggerParameterContext.ParameterType, logger);
            context.ChangeParameters([parameter, .. context.Parameters]);

            next(context);
        }
    }
}