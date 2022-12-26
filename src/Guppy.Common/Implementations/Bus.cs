using Guppy.Common.Extensions;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal class Bus : IBus, IBroker, IDisposable
    {
        private const int DefaultQueue = 0;

        private readonly IBroker _broker;
        private readonly IBusQueue[] _queues;
        private readonly Dictionary<Type, IBusQueue> _typeMap;
        private readonly IBusQueue _default;
        private readonly IFiltered<ISubscriber> _subscribers;

        public IPublisher this[Type type] => _broker[type];

        public Guid Id { get; } = Guid.NewGuid();

        public Bus(
            IFiltered<ISubscriber> subscribers,
            IOptions<BusConfiguration> config)
        {
            _subscribers = subscribers;
            _broker = new Broker();

            _queues = config.Value.TypeQueues.Select(x => x.Queue).Concat(DefaultQueue.Yield())
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new BusQueue(x))
                .ToArray();

            _default = this.GetQueue(DefaultQueue);

            _typeMap = config.Value.TypeQueues.ToDictionary(
                keySelector: x => x.Type,
                elementSelector: x => this.GetQueue(x.Queue));
        }

        public void Initialize()
        {
            foreach (ISubscriber subscriber in _subscribers.Instances.Sort())
            {
                this.SubscribeAll(subscriber);
            }
        }

        public void Dispose()
        {
            foreach (ISubscriber subscriber in _subscribers.Instances)
            {
                this.UnsubscribeAll(subscriber);
            }

            _broker.Dispose();
        }

        public void Flush()
        {
            foreach(var queue in _queues)
            {
                queue.Flush(_broker);
            }
        }

        public void Publish(in IMessage message)
        {
            this.GetQueue(message.PublishType).Enqueue(message);
        }

        public void Subscribe<T>(ISubscriber<T> subscriber)
            where T : notnull, IMessage
        {
            _broker.Subscribe(subscriber);
        }

        public void Unsubscribe<T>(ISubscriber<T> processor)

            where T : notnull, IMessage
        {
            _broker.Unsubscribe(processor);
        }

        private IBusQueue GetQueue(Type type)
        {
            if(_typeMap.TryGetValue(type, out var queue))
            {
                return queue;
            }

            return _default;
        }

        private IBusQueue GetQueue(int id)
        {
            return _queues.FirstOrDefault(x => x.Id == id) ?? _default;
        }
    }
}
