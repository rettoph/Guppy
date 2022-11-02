using Guppy.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal sealed class Scoped<T> : IScoped<T>, IDisposable
        where T : notnull
    {
        public event OnEventDelegate<IDisposable>? OnDispose;

        public T Instance { get; private set; }

        public IServiceScope Scope { get; private set; }

        public Scoped(IServiceProvider provider)
        {
            this.Scope = provider.CreateScope();
            this.Instance = this.Scope.ServiceProvider.GetRequiredService<T>();
        }

        

        public void Dispose()
        {
            this.OnDispose?.Invoke(this);

            this.Scope.Dispose();
        }
    }
}
