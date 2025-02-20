using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core
{
    public class GuppyScope(
        GuppyRoot root,
        ILifetimeScope autofac,
        IScopeVariableService scopeVariableService) : IGuppyScope
    {
        private readonly GuppyRoot _root = root;
        private readonly ILifetimeScope _autofac = autofac;
        private readonly Lazy<IScopedSystemService> _systemService = autofac.Resolve<Lazy<IScopedSystemService>>();
        public IScopeVariableService Variables { get; } = scopeVariableService;
        private bool _disposedValue;

        public IGuppyRoot Root => this._root;

        public IScopedSystemService Systems => this._systemService.Value;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this._root.RemoveScope(this);
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public T Resolve<T>()
            where T : class
        {
            return this._autofac.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return this._autofac.Resolve(type);
        }
    }
}
