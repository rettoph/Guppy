using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public interface IGuppy : IDisposable
    {
        event OnEventDelegate<IDisposable>? OnDispose;

        void Initialize(ILifetimeScope scope);
    }
}
