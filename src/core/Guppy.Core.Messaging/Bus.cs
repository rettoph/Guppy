﻿using Guppy.Core.Common;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Implementations;

namespace Guppy.Core.Messaging
{
    internal class Bus : Broker<IMessage>, IBus
    {
        private readonly (Type type, int queue)[] _queueConfigurations;
        private readonly Dictionary<Type, Queue<IMessage>> _typeQueues;
        private readonly Dictionary<int, Queue<IMessage>> _queuesDict;
        private readonly Queue<IMessage>[] _queuesArray;

        public Bus(IConfiguration<BusConfiguration> configuration)
        {
            _typeQueues = [];
            _queueConfigurations = configuration.Value.GetQueueConfigurations();

            _queuesDict = _queueConfigurations.Select(x => x.queue)
                .Concat(0.Yield())
                .Distinct()
                .ToDictionary(x => x, x => new Queue<IMessage>());

            _queuesArray = _queuesDict.OrderBy(x => x.Key)
                .Select(x => x.Value)
                .ToArray();
        }

        public void Enqueue(in IMessage message)
        {
            this.GetTypeQueue(message.Type).Enqueue(message);
        }

        public void Flush()
        {
            foreach (Queue<IMessage> queue in _queuesArray)
            {
                while (queue.TryDequeue(out IMessage? message))
                {
                    this.Publish(message);
                }
            }
        }

        private Queue<IMessage> GetTypeQueue(Type type)
        {
            if (_typeQueues.TryGetValue(type, out Queue<IMessage>? queue))
            {
                return queue;
            }

            int queueId = _queueConfigurations.FirstOrDefault(x => x.type.IsAssignableFrom(type)).queue;
            queue = _queuesDict[queueId];
            _typeQueues.Add(type, queue);

            return queue;
        }
    }
}
