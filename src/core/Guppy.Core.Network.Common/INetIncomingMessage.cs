using Guppy.Core.Messaging.Common;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common
{
    public interface INetIncomingMessage : IMessage, INetMessage, IRecyclable
    {
        ISender Sender { get; }
        INetGroup Group { get; }
        object Body { get; }
        byte Channel { get; }
        DeliveryMethod DeliveryMethod { get; }
        new INetMessageType Type { get; }

        public void Read(NetPeer sender, NetDataReader reader, ref byte channel, ref DeliveryMethod deliveryMethod);

        INetIncomingMessage Enqueue();
    }

    public interface INetIncomingMessage<T> : INetIncomingMessage
        where T : notnull
    {
        new T Body { get; }

        new INetMessageType<T> Type { get; }

        new INetIncomingMessage<T> Enqueue();
    }
}
