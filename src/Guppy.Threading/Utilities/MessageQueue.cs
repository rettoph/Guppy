using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Utilities
{
    public class MessageQueue : MessageQueue<IMessage>
    {

    }

    public class MessageQueue<TMessage> : Service
        where TMessage : class, IMessage
    {
        #region Classes
        private interface IMessageProcessor
        {
            void Process(TMessage message);
        }

        private class MessageProcessor<T> : IMessageProcessor
            where T : class, TMessage
        {
            private IMessageProcessor<T> _processor;

            public MessageProcessor(IMessageProcessor<T> processor)
            {
                _processor = processor;
            }

            public void Process(TMessage message)
            {
                if (message is T casted)
                {
                    _processor.Process(casted);
                    return;
                }

                throw new ArgumentException(nameof(message));
            }
        }
        #endregion

        #region Private Fields
        private Queue<TMessage> _queue;
        private Dictionary<Type, IMessageProcessor> _processors;
        #endregion

        #region Constructors
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _queue = new Queue<TMessage>();
            _processors = new Dictionary<Type, IMessageProcessor>();
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _queue.Clear();
            _processors.Clear();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Register a new processor into the queue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void RegisterProcessor<T>(IMessageProcessor<T> processor)
            where T : class, TMessage
        {
            _processors.Add(typeof(T), new MessageProcessor<T>(processor));
        }

        /// <summary>
        /// Deregister a processor from the queue.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void DeregisterProcessor<T>()
            where T : class, TMessage
        {
            _processors.Remove(typeof(T));
        }

        /// <summary>
        /// Process an incoming message immidiately.
        /// </summary>
        /// <param name="message"></param>
        public void Process(TMessage message)
        {
            if (_processors.TryGetValue(message.GetType(), out IMessageProcessor processor))
            {
                processor.Process(message);
            }
            else
            {
                throw new KeyNotFoundException($"{this.GetType().GetPrettyName()}::{nameof(Process)} - Unknown type recieved:'{message.GetType().GetPrettyName()}'.");
            }
        }

        /// <summary>
        /// Process an incoming message immidiately.
        /// </summary>
        /// <param name="message"></param>
        public void Process(TMessage message, Action<TMessage> postProcessor)
        {
            if (_processors.TryGetValue(message.GetType(), out IMessageProcessor processor))
            {
                processor.Process(message);
            }
            else
            {
                throw new KeyNotFoundException($"{this.GetType().GetPrettyName()}::{nameof(Process)} - Unknown type recieved when processing message:'{message}'.");
            }

            postProcessor(message);
        }

        /// <summary>
        /// Enqueue an incoming message to be processed once 
        /// <see cref="ProcessEnqueued"/> is called
        /// </summary>
        /// <param name="message"></param>
        public void Enqueue(TMessage message)
        {
            _queue.Enqueue(message);
        }

        /// <summary>
        /// Process all enqueued Dtos.
        /// </summary>
        public void ProcessEnqueued()
        {
            while(_queue.TryDequeue(out TMessage message))
            {
                this.Process(message);
            }
        }

        /// <summary>
        /// Process all enqueued Dtos.
        /// </summary>
        public void ProcessEnqueued(Action<TMessage> postProcessor)
        {
            while (_queue.TryDequeue(out TMessage message))
            {
                this.Process(message, postProcessor);
            }
        }
        #endregion
    }
}
