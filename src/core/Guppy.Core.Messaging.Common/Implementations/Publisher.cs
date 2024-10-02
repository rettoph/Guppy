namespace Guppy.Core.Messaging.Common.Implementations
{
    internal abstract class Publisher<TBase>(Type type)
        where TBase : class, IMessage
    {
        public readonly Type Type = type;

        public abstract void Publish(in TBase message);

        public abstract void TrySubscribe(IBaseSubscriber<TBase> subscriber);
        public abstract void TryUnsubscribe(IBaseSubscriber<TBase> subscriber);

        public void TrySubscribe(IEnumerable<IBaseSubscriber<TBase>> subscribers)
        {
            foreach (var subscriber in subscribers)
            {
                this.TrySubscribe(subscriber);
            }
        }

        public void TryUnsubscribe(IEnumerable<IBaseSubscriber<TBase>> subscribers)
        {
            foreach (var subscriber in subscribers)
            {
                this.TryUnsubscribe(subscriber);
            }
        }
    }

    internal class Publisher<TBase, TMessage> : Publisher<TBase>
        where TBase : class, IMessage
        where TMessage : TBase
    {
        private readonly List<IBaseSubscriber<TBase, TMessage>> _subscribers;

        public Publisher(IEnumerable<IBaseSubscriber<TBase>> subscribers) : base(typeof(TMessage))
        {
            _subscribers = new List<IBaseSubscriber<TBase, TMessage>>();

            this.TrySubscribe(subscribers);
        }

        public override void Publish(in TBase message)
        {
            Guid messageid = Guid.NewGuid();
            if (message is TMessage casted)
            {
                foreach (IBaseSubscriber<TBase, TMessage> subscriber in _subscribers)
                {
                    subscriber.Process(in messageid, casted);
                }
            }
        }

        public override void TrySubscribe(IBaseSubscriber<TBase> subscriber)
        {
            if (subscriber is IBaseSubscriber<TBase, TMessage> casted)
            {
                _subscribers.Add(casted);
            }
        }

        public override void TryUnsubscribe(IBaseSubscriber<TBase> subscriber)
        {
            if (subscriber is IBaseSubscriber<TBase, TMessage> casted)
            {
                _subscribers.Remove(casted);
            }
        }
    }
}
