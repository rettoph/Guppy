using Autofac;
using Autofac.Core.Resolving.Pipeline;
using Guppy.Core.Common.Providers;
using Serilog;

namespace Guppy.Core.Middleware
{
    internal class LoggerMiddleware(Type context) : IResolveMiddleware
    {
        private readonly Type _context = context;

        public PipelinePhase Phase => PipelinePhase.ParameterSelection;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            ILogger logger = context.Resolve<ILoggerService>().GetOrCreate(this._context);
            context.ChangeParameters(TypedParameter.From(logger).Yield().Concat(context.Parameters));

            next(context);
        }
    }
}