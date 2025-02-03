﻿using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;

namespace Guppy.Core
{
    public class GuppyScope(
        IGuppyScope? parentScope,
        ILifetimeScope autofac,
        IScopeVariableService scopeVariableService,
        IEnvironmentVariableService environmentVariableService) : IGuppyScope
    {
        private readonly ILifetimeScope _autofac = autofac;
        private readonly List<IGuppyScope> _children = [];
        public IScopeVariableService Variables { get; } = scopeVariableService;
        public IEnvironmentVariableService EnvironmentVariables { get; } = environmentVariableService;
        private bool _disposedValue;

        public IGuppyScope? Parent { get; } = parentScope;

        public IEnumerable<IGuppyScope> Children => this._children;

        public IScopedSystemService Systems { get; } = autofac.Resolve<IScopedSystemService>();

        public IGuppyScope CreateChildScope(Action<IGuppyScopeBuilder>? builder)
        {
            ILifetimeScope autofac = this._autofac.BeginLifetimeScope(containerBuilder =>
            {
                IGuppyScopeBuilder guppyScopeBuilder = new GuppyScopeBuilder(GuppyScopeTypeEnum.Child, this.EnvironmentVariables, this, containerBuilder);

                // Run custom builder
                builder?.Invoke(guppyScopeBuilder);
            });

            IGuppyScope child = autofac.Resolve<IGuppyScope>();
            this._children.Add(child);

            return child;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    foreach (IGuppyScope child in this.Children)
                    {
                        child.Dispose();
                    }
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

        public T ResolveService<T>()
            where T : notnull
        {
            return this._autofac.Resolve<T>();
        }

        public T? ResolveOptionalService<T>()
            where T : class
        {
            return this._autofac.ResolveOptional<T>();
        }

        public object ResolveService(Type type)
        {
            return this._autofac.Resolve(type);
        }

        public bool TryResolveService<T>(out T? instance)
            where T : class
        {
            return this._autofac.TryResolve<T>(out instance);
        }

        T? IGuppyVariableProvider<IScopeVariable>.GetVariable<T>() where T : default
        {
            this.Variables.TryGet<T>(out T? value);
            return value;
        }
    }
}
