using Autofac;

namespace Guppy.Engine.Common
{
    public interface IScoped<out T> : IDisposable
        where T : notnull
    {
        public T Instance { get; }
        public ILifetimeScope Scope { get; }
    }
}
