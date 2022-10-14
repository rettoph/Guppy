using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public class Broker<TBase> : IBroker<TBase>
        where TBase : notnull
    {
        private Dictionary<Type, IPublisher<TBase>> _publishers;

        public IPublisher<TBase> this[Type type] => _publishers[type];

        public Broker()
        {
            _publishers = new Dictionary<Type, IPublisher<TBase>>();
        }

        public void Subscribe<T>(ISubscriber<T> subscriber) 
            where T : TBase
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher<TBase>? publishers) && publishers is IPublisher<T, TBase> casted)
            {
                casted.Subscribe(subscriber);
                return;
            }

            _publishers.Add(typeof(T), new Publisher<T, TBase>(subscriber));
        }

        public void Unsubscribe<T>(ISubscriber<T> processor) 
            where T : TBase
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher<TBase>? publishers) && publishers is IPublisher<T, TBase> casted)
            {
                casted.Unsubscribe(processor);
            }
        }

        public void Publish(Type type, in TBase message)
        {
            if (_publishers.TryGetValue(type, out IPublisher<TBase>? publishers))
            {
                publishers.Publish(in message);
            }
        }

        public void Publish<T>(in T message)
            where T : TBase
        {
            if (_publishers.TryGetValue(typeof(T), out IPublisher<TBase>? publishers) && publishers is IPublisher<T, TBase> casted)
            {
                casted.Publish(in message);
            }
        }

        public IEnumerator<IPublisher<TBase>> GetEnumerator()
        {
            return _publishers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
