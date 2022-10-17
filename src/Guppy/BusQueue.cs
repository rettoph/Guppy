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
        where T : notnull
    {
        private struct TypeMessage
        {
            public Type Type;
            public T Message;
        }

        private ConcurrentQueue<TypeMessage> _queue;

        public int Id { get; }

        public BusQueue(int id)
        {
            this.Id = id;

            _queue = new ConcurrentQueue<TypeMessage>();
        }

        public void Enqueue(Type type, T message)
        {
            _queue.Enqueue(new TypeMessage()
            {
                Type = type,
                Message = message
            });
        }

        public void Flush(IBroker<T> broker)
        {
            int errors = 0;
            
            while(_queue.Count != 0 && errors != 5)
            {
                if(_queue.TryDequeue(out var typeMessage))
                {
                    if(typeMessage.Message is IDisposable disposable)
                    {
                        using (disposable)
                        {
                            broker.Publish(typeMessage.Type, in typeMessage.Message);
                            continue;
                        }
                    }

                    broker.Publish(typeMessage.Type, in typeMessage.Message);
                    continue;
                }
            
                errors++;
            }
        }
    }
}
