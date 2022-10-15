using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    internal sealed class Global<T> : IGlobal<T>
    {
        public T Instance { get; }

        public Global(T instance)
        {
            this.Instance = instance;
        }
    }
}
