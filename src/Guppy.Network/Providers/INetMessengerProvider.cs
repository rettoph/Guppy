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
        NetIncomingMessage CreateIncoming(NetDataReader reader);
        NetOutgoingMessage<T> CreateOutgoing<T>(NetScope scope, INetTarget target, in T content);

        NetMessenger<T> GetMessenger<T>();
        NetMessenger GetMessenger(ushort id);

        bool TryGetMessenger<T>([MaybeNullWhen(false)] out NetMessenger<T> messenger);
        bool TryGetMessenger(ushort id, [MaybeNullWhen(false)] out NetMessenger messenger);
    }
}
