using Guppy.Network.Components;
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
    public interface INetMessageProvider : IEnumerable<NetMessageFactory>
    {
        NetIncomingMessage CreateIncoming(NetPeer sender, NetDataReader reader);
        NetOutgoingMessage<T> CreateOutgoing<T>(NetMessenger messenger, in T content);

        NetMessageFactory<T> GetFactory<T>();
        NetMessageFactory GetFactory(ushort id);

        bool TryGetFactory<T>([MaybeNullWhen(false)] out NetMessageFactory<T> messenger);
        bool TryGetFactory(ushort id, [MaybeNullWhen(false)] out NetMessageFactory messenger);
    }
}
