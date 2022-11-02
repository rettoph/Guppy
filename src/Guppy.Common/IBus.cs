using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBus : IBroker
    {
        Guid Id { get; }

        void Flush();
    }
}
