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
    public class Publisher<TData> : Service
        where TData : class, IData
    {
        #region Classes
        public interface IDataPublisher
        {
            public Type Type { get; }
            UInt32 Count { get; }

            Boolean Process(TData data);
        }

        private class DataPublisher<T> : IDataPublisher
            where T : class, TData
        {
            private delegate Boolean ProcessDelegate(T message);

            private ProcessDelegate _processors;

            public Type Type => typeof(T);
            public UInt32 Count { get; private set; }

            public DataPublisher(IDataProcessor<T> processor)
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

#if DEBUG
                    if(success)
                    {
                        this.Count++;
                    }
#endif

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
        private Dictionary<Type, IDataPublisher> _publishers;
        #endregion

        #region Public Properties
        public IEnumerable<IDataPublisher> Publishers => _publishers.Values;
        #endregion


        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _log);

            _publishers = new Dictionary<Type, IDataPublisher>();
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
            if (_publishers.TryGetValue(typeof(T), out IDataPublisher processors)
                && processors is DataPublisher<T> casted)
            {
                casted.RegisterProcessor(processor);
                return;
            }

            _publishers.Add(typeof(T), new DataPublisher<T>(processor));
        }

        /// <summary>
        /// Deregister a processor from the queue.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void DeregisterProcessor<T>(IDataProcessor<T> processor)
            where T : class, TData
        {
            if (_publishers.TryGetValue(typeof(T), out IDataPublisher processors)
                && processors is DataPublisher<T> casted)
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
            if (_publishers.TryGetValue(message.GetType(), out IDataPublisher processor))
            {
                return processor.Process(message);
            }

            return false;
        }
        #endregion
    }
}
