using Guppy.Common.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    /// <summary>
    /// TODO: Refactor this, TypeMessage struct and
    /// IDisposable check? 
    /// 
    /// The IDisposable check exists soley to recycle
    /// INetMessage instances passed in here, but idk if
    /// thats even beneficial. More testing needed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class BusQueue<T> : IBusQueue<T>
        where T : notnull, IMessage
    {
        private ConcurrentQueue<T> _queue;

        public int Id { get; }

        public BusQueue(int id)
        {
            this.Id = id;

            _queue = new ConcurrentQueue<T>();
        }

        public void Enqueue(in T message)
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
                    if(message is IDisposable disposable)
                    {
                        using (disposable)
                        {
                            broker.Publish(in message);
                            continue;
                        }
                    }

                    broker.Publish(message);
                    continue;
                }
            
                errors++;
            }
        }
    }
}
