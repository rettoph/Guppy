using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public interface IRecyclable : IDisposable
    {
        void Recycle();
    }
}
