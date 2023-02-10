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
            var scope = _provider.CreateScope();
            var guppy = scope.ServiceProvider.GetRequiredService<T>();

            this.Configure(guppy, scope);

            return guppy;
        }

        IGuppy IGuppyProvider.Create(Type guppyType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            var scope = _provider.CreateScope();
            var guppy = scope.ServiceProvider.GetRequiredService(guppyType) as IGuppy;
            
            if(guppy is null)
            {
                throw new NotImplementedException();
            }

            this.Configure(guppy, scope);

            return guppy;
        }

        private void Configure(IGuppy guppy, IServiceScope scope)
        {
            guppy.OnDispose += this.HandleGuppyDisposed;

            foreach (var loader in scope.ServiceProvider.GetServices<IGuppyLoader>())
            {
                loader.Load(guppy);
            }

            guppy.Initialize(scope.ServiceProvider);

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
