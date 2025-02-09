namespace Guppy.Core.Messaging.Common.Services
{
    public interface IMessageBusService
    {
        IMessageBus Create();

        IEnumerable<IMessageBus> GetAll();

        void PublishAll<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message)
            where TSequenceGroup : unmanaged, Enum;

        void PublishAll<TMessage>(in TMessage message)
            where TMessage : IMessage;

        void EnqueueAll<TMessage>(in TMessage message)
            where TMessage : IMessage;

        void EnqueueAll<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message)
            where TSequenceGroup : unmanaged, Enum;
    }
}
