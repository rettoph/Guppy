using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public interface INetMessageService
    {
        void Send(NetOutgoingMessage message);
        void Enqueue(NetOutgoingMessage message);
        void Enqueue(NetIncomingMessage message);
        NetOutgoingMessage<T> Create<T>(NetTarget target, in T message);
    }
}
