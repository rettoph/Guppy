using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public interface IMessageService : IBroker, IDisposable
    {
        NetOutgoingMessage<T> CreateOutgoing<T>(in T content);
        NetOutgoingMessage<T> CreateOutgoing<T>(T content)
        {
            return this.CreateOutgoing<T>(in content);
        }

        void ProcessIncoming(NetIncomingMessage message);
    }
}
