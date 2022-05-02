using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetMessengerProvider : IEnumerable<NetMessenger>
    {
        NetIncomingMessage ReadIncoming(NetDataReader reader);
        NetOutgoingMessage<T> CreateOutgoing<T>(Room room, in T content);

        bool TryGetMessenger<T>([MaybeNullWhen(false)] out NetMessenger<T> messenger);
        bool TryGetMessenger(ushort id, [MaybeNullWhen(false)] out NetMessenger messenger);
    }
}
