using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Services
{
    public interface INetMessageService
    {
        NetMessageType GetById(byte id);

        NetMessageType<T> Get<T>()
            where T : notnull;

        INetIncomingMessage Read(NetPeer sender, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod);

        INetOutgoingMessage<T> Create<T>(in INetGroup group, in T body)
            where T : notnull;

        INetOutgoingMessage<T> Create<T>(in byte groupId, in T body)
            where T : notnull;
    }
}
