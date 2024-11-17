using Serilog;

namespace Guppy.Core.Common.Providers
{
    public interface ILoggerProvider
    {
        ILogger Get(Type? contextType = null);
        ILogger Get<TContext>()
        {
            return this.Get(typeof(TContext));
        }

        Lazy<ILogger> GetLazy(Type contextType)
        {
            return new Lazy<ILogger>(() => this.Get(contextType));
        }
        Lazy<ILogger> GetLazy<TContext>()
        {
            return new Lazy<ILogger>(() => this.Get<TContext>());
        }
    }
}
