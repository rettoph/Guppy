using Autofac;
using Autofac.Core.Resolving.Pipeline;
using Guppy.Engine.Providers;
using Serilog;

namespace Guppy.Engine.Middleware
{
    internal class ContextualLoggerMiddleware(Type context) : IResolveMiddleware
    {
        private readonly Type _context = context;

        public PipelinePhase Phase => PipelinePhase.ParameterSelection;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            ILogger logger = context.Resolve<ContextualLoggerProvider>().Get(_context);
            context.ChangeParameters(TypedParameter.From(logger).Yield().Concat(context.Parameters));

            next(context);
        }
    }
}
