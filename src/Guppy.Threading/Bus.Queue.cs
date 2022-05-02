using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public partial class Bus
    {
        public class Queue
        {
            private ConcurrentQueue<object> _queue;
            private Broker _broker;

            public readonly int Priority;

            internal Queue(Broker broker, int priority)
            {
                _queue = new ConcurrentQueue<object>();
                _broker = broker;

                this.Priority = priority;
            }

            public void Enqueue(object message)
            {
                _queue.Enqueue(message);
            }

            public void Publish()
            {
                while (_queue.TryDequeue(out object message))
                {
                    _broker.Publish(message);
                }
            }
        }
    }
}
