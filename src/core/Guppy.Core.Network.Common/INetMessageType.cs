using LiteNetLib;

namespace Guppy.Core.Network.Common
{
    public interface INetMessageType
    {
        byte Id { get; }
        Type Body { get; }
        DeliveryMethod DefaultDeliveryMethod { get; }
        byte DefaultOutgoingChannel { get; }

        public abstract INetIncomingMessage CreateIncoming();
    }

    public interface INetMessageType<T> : INetMessageType
        where T : notnull
    {
        new INetIncomingMessage<T> CreateIncoming();

        INetOutgoingMessage<T> CreateOutgoing();

        void Recycle(INetOutgoingMessage<T> message);

        void Recycle(INetIncomingMessage<T> message);
    }
}