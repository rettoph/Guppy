using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public abstract class MessageService : IDisposable
    {
        public abstract void Dispose();

        public abstract NetOutgoingMessage<T> CreateOutgoing<T>(in T message);

        public abstract void ProcessIncoming(NetIncomingMessage message);
    }
}
