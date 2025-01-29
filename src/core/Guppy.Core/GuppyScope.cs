using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Enums;

namespace Guppy.Core
{
    public class GuppyScope(IGuppyScope? parentScope, GuppyScopeTypeEnum type, ILifetimeScope autofac) : IGuppyScope
    {
        private readonly ILifetimeScope _autofac = autofac;
        private readonly List<IGuppyScope> _children = [];
        private bool _disposedValue;

        public IGuppyScope? Parent { get; } = parentScope;

        public IEnumerable<IGuppyScope> Children => this._children;

        public GuppyScopeTypeEnum Type { get; } = type;

        public IGuppyScope CreateChildScope(Action<IGuppyScopeBuilder>? builder)
        {
            ILifetimeScope autofac = this._autofac.BeginLifetimeScope(containerBuilder =>
            {
                IGuppyScopeBuilder guppyScopeBuilder = new GuppyScopeBuilder([], GuppyScopeTypeEnum.Child, this, containerBuilder);

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

        public T Resolve<T>()
            where T : notnull
        {
            return this._autofac.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return this._autofac.Resolve(type);
        }

        public bool TryResolve<T>(out T? instance)
            where T : class
        {
            return this._autofac.TryResolve<T>(out instance);
        }
    }
}
