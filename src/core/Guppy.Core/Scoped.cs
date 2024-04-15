using Autofac;
using Guppy.Core.Common;

namespace Guppy.Core
{
    internal sealed class Scoped<T> : IScoped<T>, IDisposable
        where T : notnull
    {
        public event OnEventDelegate<IDisposable>? OnDispose;

        public T Instance { get; private set; }

        public ILifetimeScope Scope { get; private set; }

        public Scoped(ILifetimeScope scope)
        {
            this.Scope = scope.BeginLifetimeScope();
            this.Instance = this.Scope.Resolve<T>();
        }

        public void Dispose()
        {
            this.Scope.Dispose();
            this.OnDispose?.Invoke(this);
        }
    }
}
