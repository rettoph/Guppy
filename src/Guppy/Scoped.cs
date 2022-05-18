using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    internal sealed class Scoped<T> : IScoped<T>
    {
        public Scoped(T instance)
        {
            this.Instance = instance;
        }

        public T Instance { get; }
    }
}
