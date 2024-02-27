using Guppy.Messaging;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Network
{
    public interface INetIncomingMessage : IMessage, INetMessage, IRecyclable
    {
        INetGroup Group { get; }
        object Body { get; }
        byte Channel { get; }
        DeliveryMethod DeliveryMethod { get; }
        new NetMessageType Type { get; }

        public void Read(NetDataReader reader, ref byte channel, ref DeliveryMethod deliveryMethod);

        INetIncomingMessage Enqueue();
    }

    public interface INetIncomingMessage<T> : INetIncomingMessage
        where T : notnull
    {
        new T Body { get; }

        new NetMessageType<T> Type { get; }

        new INetIncomingMessage<T> Enqueue();
    }
}
