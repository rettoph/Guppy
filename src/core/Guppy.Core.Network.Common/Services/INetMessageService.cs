using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Services
{
    public interface INetMessageService
    {
        INetMessageType GetById(byte id);

        INetMessageType<T> Get<T>()
            where T : notnull;

        INetIncomingMessage Read(NetPeer sender, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod);

        INetOutgoingMessage<T> Create<T>(in INetGroup group, in T body)
            where T : notnull;
    }
}
