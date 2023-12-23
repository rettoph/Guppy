using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Network.Services
{
    public interface INetMessageService
    {
        internal void Initialize(NetScope netScope);

        NetMessageType Get(byte id);

        NetMessageType<T> Get<T>()
            where T : notnull;

        INetIncomingMessage Read(NetPeer? peer, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod);

        INetOutgoingMessage<T> Create<T>(in T body)
            where T : notnull;
    }
}
