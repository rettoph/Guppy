﻿using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Messaging
{
    public class BusConfiguration
    {
        private const int DefaultPriority = 0;

        private readonly List<(Type type, int queue, int priority)> _queues = [];

        public void SetQueue<T>(int queue, int priority = DefaultPriority)
            where T : IMessage
        {
            this.SetQueue(typeof(T), queue, priority);
        }

        public void SetQueue(Type type, int queue, int priority = DefaultPriority)
        {
            _queues.Add((type, queue, priority));
        }

        public (Type type, int queue)[] GetQueueConfigurations()
        {
            return _queues.OrderByDescending(x => x.priority).Select(x => (x.type, x.queue)).ToArray();
        }
    }
}
