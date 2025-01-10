using System.Runtime.InteropServices;

namespace Guppy.Core.Messaging.Common.Implementations
{
    public class Broker<TBase> : IBroker<TBase>
        where TBase : class, IMessage
    {
        private readonly List<IBaseSubscriber<TBase>> _subscribers;
        private readonly Dictionary<Type, Publisher<TBase>> _publishers;

        public Broker()
        {
            this._subscribers = [];
            this._publishers = [];
        }

        public void Publish(in TBase message)
        {
            this.GetPublisher(message.Type).Publish(message);
        }

        public void Subscribe(IBaseSubscriber<TBase> subscriber)
        {
            this._subscribers.Add(subscriber);
            foreach (Publisher<TBase> publisher in this._publishers.Values)
            {
                publisher.TrySubscribe(subscriber);
            }
        }

        public bool TrySubscribe(IBaseSubscriber subscriber)
        {
            if (subscriber is IBaseSubscriber<TBase> casted == false)
            {
                return false;
            }

            this.Subscribe(casted);
            return true;
        }

        public bool TryUnsubscribe(IBaseSubscriber subscriber)
        {
            if (subscriber is IBaseSubscriber<TBase> casted == false)
            {
                return false;
            }

            this.Unsubscribe(casted);
            return true;
        }

        public void Unsubscribe(IBaseSubscriber<TBase> subscriber)
        {
            if (this._subscribers.Remove(subscriber))
            {
                foreach (Publisher<TBase> publisher in this._publishers.Values)
                {
                    publisher.TryUnsubscribe(subscriber);
                }
            }
        }

        private Publisher<TBase> GetPublisher(Type type)
        {
            ref Publisher<TBase>? publisher = ref CollectionsMarshal.GetValueRefOrAddDefault(this._publishers, type, out bool exists);

            if (exists)
            {
                return publisher!;
            }

            Type publisherType = typeof(Publisher<,>).MakeGenericType(typeof(TBase), type);
            publisher = (Publisher<TBase>)Activator.CreateInstance(publisherType, this._subscribers)!;

            return publisher;
        }
    }
}