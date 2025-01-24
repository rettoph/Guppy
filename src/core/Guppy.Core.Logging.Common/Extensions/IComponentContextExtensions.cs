using Autofac;
using Guppy.Core.Logging.Common.Services;

namespace Guppy.Core.Logging.Common.Extensions
{
    public static class IComponentContextExtensions
    {
        public static ILogger ResolveLogger(this IComponentContext context, Type loggerContext)
        {
            return context.Resolve<ILoggerService>().GetLogger(loggerContext);
        }

        public static ILogger ResolveLogger<TContext>(this IComponentContext context)
        {
            return context.Resolve<ILoggerService>().GetLogger<TContext>();
        }
    }
}
