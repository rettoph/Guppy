using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public sealed class Orderable<T>
    {
        public readonly T Instance;
        public readonly int Order;

        public Orderable(T instance, int order)
        {
            this.Instance = instance;
            this.Order = order;
        }
    }
}
