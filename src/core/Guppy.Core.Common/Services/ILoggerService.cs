using Serilog;

namespace Guppy.Core.Common.Providers
{
    public interface ILoggerService
    {
        ILogger GetOrCreate(Type? contextType = null);
        ILogger GetOrCreate<TContext>() => this.GetOrCreate(typeof(TContext));

        Lazy<ILogger> GetOrCreateLazy(Type contextType) => new(() => this.GetOrCreate(contextType));
        Lazy<ILogger> GetOrCreateLazy<TContext>() => new(this.GetOrCreate<TContext>);
    }
}