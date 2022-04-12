using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetMessengerProvider
    {
        NetIncomingMessage ReadIncoming(NetDataReader reader);
        NetOutgoingMessage<T> CreateOutgoing<T>(in byte roomId, in T content);

        bool TryGetMessenger<T>(out NetMessenger<T>? messenger);
        bool TryGetMessenger(ushort id, out NetMessenger messenger);
    }
}
