using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public interface INetScopeOutgoingMessageService
    {
        NetOutgoingMessage<T> Create<T>(INetTarget target, in T content);

        void Enqueue(NetOutgoingMessage message);
        void Send(int maximum);
    }
}
