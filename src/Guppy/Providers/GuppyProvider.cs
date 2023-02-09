using Guppy.Common;
using Guppy.Common.Collections;
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

        public GuppyProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        T IGuppyProvider.Create<T>()
        {
            var scoped = _provider.GetRequiredService<IScoped<T>>();

            scoped.Instance.OnDispose += this.HandleGuppyDisposed;

            scoped.Instance.Initialize(scoped.Scope.ServiceProvider);

            this.Add(scoped.Instance);

            return scoped.Instance;
        }

        private void HandleGuppyDisposed(IDisposable args)
        {
            if(args is IGuppy guppy)
            {
                guppy.OnDispose -= this.HandleGuppyDisposed;

                this.Remove(guppy);
            }
        }
    }
}
