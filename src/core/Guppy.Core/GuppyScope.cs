using Autofac;
using Guppy.Core.Common;

namespace Guppy.Core
{
    public class GuppyScope(IGuppyScope? parentScope, ILifetimeScope autofac) : IGuppyScope
    {
        private readonly ILifetimeScope _autofac = autofac;
        private readonly List<IGuppyScope> _children = [];
        private bool _disposedValue;

        public IGuppyScope? Parent { get; private set; } = parentScope;

        public IEnumerable<IGuppyScope> Children => this._children;

        public bool IsRoot => this.Parent is null;

        public IGuppyScope Root => this.IsRoot ? this : this.Parent!;

        public IGuppyScope CreateChildScope(Action<IGuppyScopeBuilder> builder)
        {
            ILifetimeScope autofac = this._autofac.BeginLifetimeScope(containerBuilder =>
            {
                IGuppyScopeBuilder guppyScopeBuilder = new GuppyScopeBuilder(this, containerBuilder);
                guppyScopeBuilder.Register<IGuppyScope>((ILifetimeScope scope) => new GuppyScope(this, scope));

                builder(guppyScopeBuilder);
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
    }
}
