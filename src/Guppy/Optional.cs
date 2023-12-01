using Autofac;
using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    internal class Optional<T> : IOptional<T>
        where T : class
    {
        public T? Value { get; }

        public Optional(ILifetimeScope scope)
        {
            this.Value = scope.ResolveOptional<T>();
        }
    }
}
