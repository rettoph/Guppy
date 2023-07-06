using Autofac;

namespace Guppy.Common
{
    public interface IScoped<out T> : IDisposable
        where T : notnull
    {
        public T Instance { get; }
        public ILifetimeScope Scope { get; }
    }
}
