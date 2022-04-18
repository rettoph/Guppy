using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Definitions
{
    public abstract class BusMessageDefinition
    {
        public abstract Type Type { get; }
        public abstract int Queue { get; }
    }

    public abstract class BusMessageDefinition<T> : BusMessageDefinition
    {
        public override Type Type { get; } = typeof(T);
    }
}
