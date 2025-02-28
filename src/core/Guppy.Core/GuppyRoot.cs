using System.Collections.ObjectModel;
using Autofac;
using Guppy.Core.Builders;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Services;

namespace Guppy.Core
{
    public class GuppyRoot : IGuppyRoot
    {
        private readonly ILifetimeScope _autofac;
        private readonly List<IGuppyScope> _scopes;
        private bool _disposed;

        public ReadOnlyCollection<IGuppyScope> Scopes { get; }

        public IEnvironmentVariableService EnvironmentVariables { get; }

        public IGuppyRoot Root => this;

        public IScopeVariableService Variables { get; }

        public IScopedSystemService Systems { get; }

        public GuppyRoot(
            ILifetimeScope autofac,
            IEnvironmentVariableService environmentVariables,
            IScopeVariableService variables,
            IScopedSystemService systems)
        {
            this._disposed = false;
            this._autofac = autofac;
            this._scopes = [this];

            this.EnvironmentVariables = environmentVariables;
            this.Variables = variables;
            this.Systems = systems;
            this.Scopes = new ReadOnlyCollection<IGuppyScope>(this._scopes);
        }

        public IGuppyScope CreateScope(Action<IGuppyScopeBuilder>? build = null)
        {
            ILifetimeScope lifetimeScope = this._autofac.BeginLifetimeScope(containerBuilder =>
            {
                GuppyScopeBuilder scopeBuilder = new(
                    root: this,
                    scopeBuilderFilters: this.Resolve<IEnumerable<GuppyContainerBuilderFilter<IGuppyScopeBuilder>>>(),
                    containerBuilder: containerBuilder);

                build?.Invoke(scopeBuilder);

                scopeBuilder.ExcecuteFilters();
            });

            IGuppyScope guppyScope = lifetimeScope.Resolve<IGuppyScope>();
            this._scopes.Add(guppyScope);

            return guppyScope;
        }

        public void Dispose()
        {
            if (this._disposed == true)
            {
                return;
            }

            this._disposed = true;
            foreach (IGuppyScope scope in this._scopes)
            {
                scope.Dispose();
            }

            this._autofac.Dispose();
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

        public void RemoveScope(IGuppyScope scope)
        {
            this._scopes.Remove(scope);
        }
    }
}
