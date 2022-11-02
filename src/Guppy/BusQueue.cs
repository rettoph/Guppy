using Guppy.Common;
using Guppy.Common.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
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
    internal sealed class BusQueue : IBusQueue
    {
        private ConcurrentQueue<IMessage> _queue;

        public int Id { get; }

        public BusQueue(int id)
        {
            this.Id = id;

            _queue = new ConcurrentQueue<IMessage>();
        }

        public void Enqueue(in IMessage message)
        {
            _queue.Enqueue(message);
        }

        public void Flush(IBroker broker)
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
