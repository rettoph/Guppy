using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IScoped<out T> : IDisposable
        where T : notnull
    {
        event OnEventDelegate<IDisposable>? OnDispose;

        public T Instance { get; }
        public IServiceScope Scope { get; }
    }
}
