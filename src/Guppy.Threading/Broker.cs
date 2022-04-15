using Guppy.Threading;

namespace Guppy.Threading
{
    public class Broker
    {
        #region Classes
        private abstract class TypeSubscribers
        {
            public readonly Type Type;

            public TypeSubscribers(Type type)
            {
                this.Type = type;
            }

            public abstract bool Publish(in object data);
        }

        private class TypeSubscribers<T> : TypeSubscribers
        {
            private delegate bool ProcessDelegate(in T message);

            private ProcessDelegate? _subscribers;

            public TypeSubscribers(ISubscriber<T> subscriber) : base(typeof(T))
            {
                _subscribers = subscriber.Process;
            }

            public bool Publish(in T message)
            {
                if(_subscribers is null)
                {
                    return true;
                }

                bool success = true;
                foreach (ProcessDelegate processor in _subscribers.GetInvocationList())
                {
                    success &= processor(in message);
                }

                return success;
            }
            public override bool Publish(in object message)
            {
                if (message is T casted)
                {
                    return this.Publish(casted);
                }

                throw new ArgumentException(nameof(message));
            }

            public void Subscribe(ISubscriber<T> subscriber)
            {
                _subscribers += subscriber.Process;
            }

            public void Unsubscribe(ISubscriber<T> subscriber)
            {
                _subscribers -= subscriber.Process;
            }
        }
        #endregion

        #region Private Fields
        private Dictionary<Type, TypeSubscribers> _subscribers;
        #endregion

        public Broker()
        {
            _subscribers = new Dictionary<Type, TypeSubscribers>();
        }

        #region Helper Methods
        /// <summary>
        /// Subscribe a subscriber with the broker
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subscriber"></param>
        public void Subscribe<T>(ISubscriber<T> subscriber)
        {
            if (_subscribers.TryGetValue(typeof(T), out TypeSubscribers subscribers) && subscribers is TypeSubscribers<T> casted)
            {
                casted.Subscribe(subscriber);
                return;
            }

            _subscribers.Add(typeof(T), new TypeSubscribers<T>(subscriber));
        }

        /// <summary>
        /// Unsubscribe a subscriber from the queue.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processor"></param>
        public void Unsubscribe<T>(ISubscriber<T> processor)
        {
            if (_subscribers.TryGetValue(typeof(T), out TypeSubscribers subscribers) && subscribers is TypeSubscribers<T> casted)
            {
                casted.Unsubscribe(processor);
                return;
            }
        }

        /// <summary>
        /// Publish an incoming message.
        /// </summary>
        /// <param name="message"></param>
        public bool Publish(in object message)
        {
            if (_subscribers.TryGetValue(message.GetType(), out TypeSubscribers subscribers))
            {
                return subscribers.Publish(in message);
            }

            return false;
        }

        public bool Publish<T>(in T message)
        {
            if (_subscribers.TryGetValue(typeof(T), out TypeSubscribers subscribers) && subscribers is TypeSubscribers<T> casted)
            {
                return casted.Publish(in message);
            }

            return false;
        }
        #endregion
    }
}