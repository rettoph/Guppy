using Serilog;

namespace Guppy.Core.Common.Providers
{
    public interface ILoggerProvider
    {
        ILogger Get(Type contextType);
        ILogger Get<TContext>()
        {
            return this.Get(typeof(TContext));
        }
        ILogger Get(string? context = null);

        Lazy<ILogger> GetLazy(Type contextType)
        {
            return new Lazy<ILogger>(() => this.Get(contextType));
        }
        Lazy<ILogger> GetLazy<TContext>()
        {
            return new Lazy<ILogger>(() => this.Get<TContext>());
        }
        Lazy<ILogger> GetLazy(string? context = null)
        {
            return new Lazy<ILogger>(() => this.Get(context));
        }
    }
}
