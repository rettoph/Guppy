using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    internal sealed class Lazier<T> : Lazy<T>
        where T : class
    {
        public Lazier(IServiceProvider p) : base(() => p.GetRequiredService<T>())
        {

        }
    }
}
