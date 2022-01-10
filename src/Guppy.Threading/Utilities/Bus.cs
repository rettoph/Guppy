using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Utilities
{
    public class Bus : Bus<IData>
    {
    }

    public class Bus<TData> : DataPublisher<TData>
        where TData : class, IData
    {
        #region Constants
        public const Int32 DefaultQueue = 0;
        #endregion

        #region Private Fields
        private Dictionary<Int32, ConcurrentQueue<TData>> _queues;
        private ConcurrentQueue<TData>[] _orderedQueues = Array.Empty<ConcurrentQueue<TData>>();
        private Dictionary<Type, ConcurrentQueue<TData>> _messageTypeQueues;
        private ConcurrentQueue<TData> _defaultQueue;
        private ILogger _logger;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _logger);

            _queues = new Dictionary<Int32, ConcurrentQueue<TData>>();
            _messageTypeQueues = new Dictionary<Type, ConcurrentQueue<TData>>();
            _defaultQueue = this.GetQueue(Bus.DefaultQueue);
        }
        #endregion

        #region Helper Methods
        public void Enqueue(TData message)
        {
            if (_messageTypeQueues.TryGetValue(message.GetType(), out ConcurrentQueue<TData> queue))
            {
                queue.Enqueue(message);
                return;
            }

            _defaultQueue.Enqueue(message);
        }

        public void PublishEnqueued()
        {
            foreach (ConcurrentQueue<TData> queue in _orderedQueues)
            {
                while (queue.TryDequeue(out TData message))
                {
                    this.Publish(message);
                }
            }
        }

        private ConcurrentQueue<TData> GetQueue(Int32 queue)
        {
            // Look i know theQueue isnt a great name. It makes sense that we call
            // the queue id 'queue' in the context of this method. it *is* queue N.
            // theQueue is.. the actual queue. Dont sue me
            if (!_queues.TryGetValue(queue, out ConcurrentQueue<TData> theQueue))
            {
                theQueue = new ConcurrentQueue<TData>();

                // Create the new queue
                _queues.Add(queue, theQueue);

                // Attempt to order all registered queues into an array
                _orderedQueues = _queues.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray();
            }

            return theQueue;
        }

        /// <summary>
        /// Configure which <paramref name="queue"/> the recieved message types should be placed into
        /// when passed into <see cref="Enqueue(TData)"/>. If the type is not defined, then it will be
        /// placed into the <see cref="DefaultQueue"/>.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="messageTypes"></param>
        public void ConfigureMessageTypes(Int32 queue, params Type[] messageTypes)
        {
            ConcurrentQueue<TData> theQueue = this.GetQueue(queue);

            // Create lookup table so that incoming messages can be placed into their apprioriate queue
            foreach (Type messageType in messageTypes)
            {
                _messageTypeQueues[messageType] = theQueue;
                _logger.Information("{type}::{method} - Registered message type '{messageType}' to queue '{queue}'", nameof(MessageBus), nameof(ConfigureMessageTypes), messageType.Name, queue);
                return;
            }
        }
        #endregion
    }
}
