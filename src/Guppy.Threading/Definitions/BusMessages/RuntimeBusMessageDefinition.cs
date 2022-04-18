using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Definitions.BusMessages
{
    internal sealed class RuntimeBusMessageDefinition<T> : BusMessageDefinition<T>
    {
        public override int Queue { get; }

        public RuntimeBusMessageDefinition(int queue)
        {
            this.Queue = queue;
        }
    }
}
