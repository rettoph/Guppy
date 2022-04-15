using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    internal sealed class BusMessageCollection : List<BusMessageDescriptor>, IBusMessageCollection
    {
        public IBusMessageCollection Add<T>(int queue)
        {
            this.Add(BusMessageDescriptor.Create<T>(queue));

            return this;
        }

        public Bus Build()
        {
            return new Bus(this);
        }
    }
}
