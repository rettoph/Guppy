using Guppy.Common;
using Guppy.Common.Collections;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class GuppyProvider : CollectionManager<IGuppy>, IGuppyProvider
    {
        private IServiceProvider _provider;
        private Dictionary<IGuppy, IServiceScope> _scopes;

        public GuppyProvider(IServiceProvider provider)
        {
            _provider = provider;
            _scopes = new Dictionary<IGuppy, IServiceScope>();
        }

        T IGuppyProvider.Create<T>()
        {
            var scoped = _provider.GetRequiredService<IScoped<T>>();

            this.Configure(scoped);

            return scoped.Instance;
        }

        IGuppy IGuppyProvider.Create(Type guppyType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            var scopedType = typeof(IScoped<>).MakeGenericType(guppyType);
            var scoped = _provider.GetRequiredService(scopedType) as IScoped<IGuppy> ?? throw new NotImplementedException();

            this.Configure(scoped);

            return scoped.Instance;
        }

        private void Configure(IScoped<IGuppy> scoped)
        {
            scoped.Instance.OnDispose += this.HandleGuppyDisposed;

            foreach (var loader in scoped.Scope.ServiceProvider.GetServices<IGuppyLoader>())
            {
                loader.Load(scoped.Instance);
            }

            scoped.Instance.Initialize(scoped.Scope.ServiceProvider);

            this.Add(scoped.Instance);
            _scopes.Add(scoped.Instance, scoped.Scope);
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
