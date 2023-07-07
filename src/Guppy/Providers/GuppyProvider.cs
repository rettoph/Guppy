using Autofac;
using Guppy.Common;
using Guppy.Common.Autofac;
using Guppy.Common.Collections;
using Guppy.Loaders;

namespace Guppy.Providers
{
    internal sealed class GuppyProvider : CollectionManager<IGuppy>, IGuppyProvider
    {
        private ILifetimeScope _scope;
        private Dictionary<IGuppy, ILifetimeScope> _scopes;

        public GuppyProvider(ILifetimeScope scope)
        {
            _scope = scope;
            _scopes = new Dictionary<IGuppy, ILifetimeScope>();
        }

        T IGuppyProvider.Create<T>()
        {
            var scope = _scope.BeginLifetimeScope(LifetimeScopeTags.Guppy, builder =>
            {
                builder.RegisterType<T>().AsSelf().AsImplementedInterfaces().SingleInstance();
            });
            var guppy = scope.Resolve<T>();

            this.Configure(guppy, scope);

            return guppy;
        }

        IGuppy IGuppyProvider.Create(Type guppyType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            var scope = _scope.BeginLifetimeScope(LifetimeScopeTags.Guppy, builder =>
            {
                builder.RegisterType(guppyType).AsSelf().AsImplementedInterfaces().SingleInstance();
            });
            var guppy = scope.Resolve(guppyType) as IGuppy;
            
            if(guppy is null)
            {
                throw new NotImplementedException();
            }

            this.Configure(guppy, scope);

            return guppy;
        }

        private void Configure(IGuppy guppy, ILifetimeScope scope)
        {
            guppy.OnDispose += this.HandleGuppyDisposed;

            foreach (var loader in scope.Resolve<IEnumerable<IGuppyLoader>>())
            {
                loader.Load(guppy);
            }

            guppy.Initialize(scope);

            this.Add(guppy);
            _scopes.Add(guppy, scope);
        }

        private void HandleGuppyDisposed(IDisposable args)
        {
            if(args is IGuppy guppy)
            {
                guppy.OnDispose -= this.HandleGuppyDisposed;

                this.Remove(guppy);

                if(_scopes.Remove(guppy, out var scope))
                {
                    scope.Dispose();
                }
            }
        }
    }
}
