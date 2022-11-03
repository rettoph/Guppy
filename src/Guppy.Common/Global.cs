using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    internal sealed class Global : IDisposable
    {
        public readonly IScoped<IServiceProvider> Scope;

        public Global(IScoped<IServiceProvider> provider)
        {
            this.Scope = provider;
        }

        public void Dispose()
        {
            this.Scope.Dispose();
        }
    }
}
