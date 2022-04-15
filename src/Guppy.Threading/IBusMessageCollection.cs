using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public interface IBusMessageCollection : IList<BusMessageDescriptor>
    {
        IBusMessageCollection Add<T>(int queue);
    }
}
