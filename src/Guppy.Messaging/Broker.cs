using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Messaging
{
    public class Broker<TBase> : IBroker<TBase>
        where TBase : class, IMessage
    {
        private List<IBaseSubscriber<TBase>> _subscribers;
        private Dictionary<Type, Publisher<TBase>> _publishers;

        public Broker()
        {
            _subscribers = new List<IBaseSubscriber<TBase>>();
            _publishers = new Dictionary<Type, Publisher<TBase>>();
        }

        public void Publish(in TBase message)
        {
            this.GetPublisher(message.Type).Publish(message);
        }

        public void Subscribe(IBaseSubscriber<TBase> subscriber)
        {
            _subscribers.Add(subscriber);
            foreach (Publisher<TBase> publisher in _publishers.Values)
            {
                publisher.TrySubscribe(subscriber);
            }
        }

        public void Unsubscribe(IBaseSubscriber<TBase> subscriber)
        {
            if(_subscribers.Remove(subscriber))
            {
                foreach(Publisher<TBase> publisher in _publishers.Values)
                {
                    publisher.TryUnsubscribe(subscriber);
                }
            }
        }

        private Publisher<TBase> GetPublisher(Type type)
        {
            ref Publisher<TBase>? publisher = ref CollectionsMarshal.GetValueRefOrAddDefault(_publishers, type, out bool exists);

            if (exists)
            {
                return publisher!;
            }

            Type publisherType = typeof(Publisher<,>).MakeGenericType(typeof(TBase), type);
            publisher = (Publisher<TBase>)Activator.CreateInstance(publisherType, _subscribers)!;

            return publisher;
        }
    }
}
