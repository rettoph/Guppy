using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    internal sealed class BusQueue<T> : IBusQueue<T>
        where T : notnull
    {
        private ConcurrentQueue<T> _queue;

        public int Id { get; }

        public BusQueue(int id)
        {
            this.Id = id;

            _queue = new ConcurrentQueue<T>();
        }

        public void Enqueue(T message)
        {
            _queue.Enqueue(message);
        }

        public void Flush(IBroker<T> broker)
        {
            int errors = 0;
            
            while(_queue.Count != 0 && errors != 5)
            {
                if(_queue.TryDequeue(out var message))
                {
                    broker.Publish(message.GetType(), in message);
                    continue;
                }
            
                errors++;
            }
        }
    }
}
