using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public class Bus<T> : IBus<T>, IBroker<T>
        where T : notnull, IMessage
    {
        private const int DefaultQueue = 0;

        private IBroker<T> _broker;
        private IBusQueue<T>[] _queues;
        private Dictionary<Type, IBusQueue<T>> _typeMap;
        private IBusQueue<T> _default;

        public IPublisher<T> this[Type type] => _broker[type];

        public Guid Id { get; } = Guid.NewGuid();

        public Bus(IEnumerable<BusConfiguration> config)
        {
            _broker = new Broker<T>();

            config = this.Filter(config);

            _queues = config.Select(x => x.Queue).Concat(DefaultQueue.Yield())
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new BusQueue<T>(x))
                .ToArray();

            _default = this.GetQueue(DefaultQueue);

            _typeMap = config.ToDictionary(
                keySelector: x => x.Type,
                elementSelector: x => this.GetQueue(x.Queue));
        }

        public void Flush()
        {
            foreach(var queue in _queues)
            {
                queue.Flush(_broker);
            }
        }

        public void Publish(in T message)
        {
            this.GetQueue(message.PublishType).Enqueue(message);
        }

        public void Subscribe<TMessage>(ISubscriber<TMessage> subscriber) 
            where TMessage : T
        {
            _broker.Subscribe(subscriber);
        }

        public void Unsubscribe<TMessage>(ISubscriber<TMessage> processor) 
            where TMessage : T
        {
            _broker.Unsubscribe(processor);
        }

        public IEnumerator<IPublisher<T>> GetEnumerator()
        {
            return _broker.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _broker.GetEnumerator();
        }

        private IBusQueue<T> GetQueue(Type type)
        {
            if(_typeMap.TryGetValue(type, out var queue))
            {
                return queue;
            }

            return _default;
        }

        private IBusQueue<T> GetQueue(int id)
        {
            return _queues.FirstOrDefault(x => x.Id == id) ?? _default;
        }

        private IEnumerable<BusConfiguration> Filter(IEnumerable<BusConfiguration> config)
        {
            return config.Where(x => x.Type.IsAssignableTo(typeof(T))).ToList();
        }
    }

    internal sealed class Bus : Bus<IMessage>, IBus
    {
        public Bus(IEnumerable<BusConfiguration> config) : base(config)
        {
        }
    }
}
