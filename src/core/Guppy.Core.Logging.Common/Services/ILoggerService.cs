using System.Diagnostics;

namespace Guppy.Core.Logging.Common.Services
{
    public interface ILoggerService
    {
        /// <summary>
        /// Resolve and return an <see cref="ILogger"/> instances with context
        /// of <paramref name="context"/>
        /// </summary>
        /// <param name="context">The context of the logger to resolve</param>
        /// <returns></returns>
        ILogger GetLogger(Type context);

        /// <summary>
        /// Resolve and return an <see cref="ILogger{TContext}"/> instances.
        /// </summary>
        /// <typeparam name="TContext">The context of the logger to resolve</typeparam>
        /// <returns></returns>
        ILogger<TContext> GetLogger<TContext>()
        {
            if (this.GetLogger(typeof(TContext)) is ILogger<TContext> casted)
            {
                return casted;
            }

            // This should never be possible. Somehow a logger has been cached
            // with the incorrect context type key
            throw new UnreachableException();
        }
    }
}