using Autofac;
using Guppy.Common;
using Guppy.Common.Autofac;
using Guppy.Common.Collections;
using Guppy.Loaders;
using System.Collections;

namespace Guppy.Providers
{
    internal sealed class GuppyProvider : IGuppyProvider
    {
        private ILifetimeScope _scope;
        private List<IGuppy> _guppies;
        private Dictionary<IGuppy, ILifetimeScope> _scopes;

        public event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyCreated;
        public event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyDestroyed;

        public GuppyProvider(ILifetimeScope scope)
        {
            _guppies = new List<IGuppy>();
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

            guppy.Initialize(scope);
            _scopes.Add(guppy, scope);
            _guppies.Add(guppy);

            this.OnGuppyCreated?.Invoke(this, guppy);
        }

        private void HandleGuppyDisposed(IDisposable args)
        {
            if(args is IGuppy guppy)
            {
                guppy.OnDispose -= this.HandleGuppyDisposed;

                if(_scopes.Remove(guppy, out var scope))
                {
                    scope.Dispose();
                }

                _guppies.Remove(guppy);
                this.OnGuppyDestroyed?.Invoke(this, guppy);
            }
        }

        public IEnumerator<IGuppy> GetEnumerator()
        {
            return _guppies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
