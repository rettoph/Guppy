using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Utilities
{
    public class Bus : Bus<IData>
    {
    }

    public class Bus<TData> : DataProcessor<TData>
        where TData : class, IData
    {
        #region Private Fields
        private Dictionary<Int32, Queue<TData>> _queues;
        private Queue<TData>[] _orderedQueues = Array.Empty<Queue<TData>>();
        private Dictionary<Type, Queue<TData>> _messageTypeQueues;
        private ILog _log;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _log);

            _queues = new Dictionary<Int32, Queue<TData>>();
            _messageTypeQueues = new Dictionary<Type, Queue<TData>>();
        }
        #endregion

        #region Helper Methods
        public Boolean TryEnqueue(TData message)
        {
            if (_messageTypeQueues.TryGetValue(message.GetType(), out Queue<TData> queue))
            {
                queue.Enqueue(message);
                return true;
            }

            _log.Warn($"{nameof(MessageBus)}::{nameof(TryEnqueue)} - Unknown message type '{message.GetType().GetPrettyName()}'");
            return false;
        }

        public void ProcessEnqueued()
        {
            foreach (Queue<TData> queue in _orderedQueues)
            {
                while (queue.TryDequeue(out TData message))
                {
                    this.Process(message);
                }
            }
        }

        public void RegisterMessageTypes(Int32 queue, params Type[] messageTypes)
        {
            // Look i know theQueue isnt a great name. It makes sense that we call
            // the queue id 'queue' in the context of this method. it *is* queue N.
            // theQueue is.. the actual queue. Dont sue me
            if(!_queues.TryGetValue(queue, out Queue<TData> theQueue))
            {
                theQueue = new Queue<TData>();

                // Create the new queue
                _queues.Add(queue, theQueue);

                // Attempt to order all registered queues into an array
                _orderedQueues = _queues.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray();
            }

            // Create lookup table so that incoming messages can be placed into their apprioriate queue
            foreach (Type messageType in messageTypes)
            {
                if (_messageTypeQueues.TryAdd(messageType, theQueue))
                {
                    _log.Info($"{nameof(MessageBus)}::{nameof(RegisterMessageTypes)} - Registered message type '{messageType.Name}' to queue '{queue}'.");
                    return;
                }

                _log.Warn($"{nameof(MessageBus)}::{nameof(RegisterMessageTypes)} - Unable to register message type '{messageType.Name}' to queue '{queue}'.");
            }
        }
        #endregion
    }
}
