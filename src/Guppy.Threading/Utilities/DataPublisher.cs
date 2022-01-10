using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using Guppy.EntityComponent.Interfaces;
using Guppy.Threading.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Utilities
{
    public class DataPublisher<TData> : Service
        where TData : class, IData
    {
        #region Classes
        private interface IDataProcessorContainer
        {
            Boolean Process(TData data);
        }

        private class DataProcessorContainer<T> : IDataProcessorContainer
            where T : class, TData
        {
            private delegate Boolean ProcessDelegate(T message);

            private ProcessDelegate _processors;

            public DataProcessorContainer(IDataProcessor<T> processor)
            {
                _processors = processor.Process;
            }

            public Boolean Process(TData message)
            {
                if (message is T casted)
                {
                    Boolean success = true;
                    foreach (ProcessDelegate processor in _processors.GetInvocationList())
                    {
                        success &= processor(casted);
                    }

                    return success;
                }

                throw new ArgumentException(nameof(message));
            }

            public void RegisterProcessor(IDataProcessor<T> processor)
            {
                _processors += processor.Process;
            }

            public void DeregisterProcessor(IDataProcessor<T> processor)
            {
                _processors -= processor.Process;
            }
        }
        #endregion

        #region Private Fields
        private ILogger _log;
        private Dictionary<Type, IDataProcessorContainer> _processors;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _log);

            _processors = new Dictionary<Type, IDataProcessorContainer>();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Register a new processor into the queue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void RegisterProcessor<T>(IDataProcessor<T> processor)
            where T : class, TData
        {
            if (_processors.TryGetValue(typeof(T), out IDataProcessorContainer processors)
                && processors is DataProcessorContainer<T> casted)
            {
                casted.RegisterProcessor(processor);
                return;
            }

            _processors.Add(typeof(T), new DataProcessorContainer<T>(processor));
        }

        /// <summary>
        /// Deregister a processor from the queue.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void DeregisterProcessor<T>(IDataProcessor<T> processor)
            where T : class, TData
        {
            if (_processors.TryGetValue(typeof(T), out IDataProcessorContainer processors)
                && processors is DataProcessorContainer<T> casted)
            {
                casted.DeregisterProcessor(processor);
                return;
            }
        }

        /// <summary>
        /// Publish an incoming message to be processed immidiately.
        /// </summary>
        /// <param name="message"></param>
        public Boolean Publish(TData message)
        {
            if (_processors.TryGetValue(message.GetType(), out IDataProcessorContainer processor))
            {
                return processor.Process(message);
            }

            return false;
        }
        #endregion
    }
}
