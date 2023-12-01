using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Messaging
{
    public interface IBus : IBroker<IMessage>
    {
        void Enqueue(in IMessage message);

        void Flush();
    }
}
