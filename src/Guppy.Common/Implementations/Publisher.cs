using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal abstract class Publisher<TBase> : IPublisher<TBase>
        where TBase : IMessage
    {
        public Type Type { get; }

        public Publisher(Type type)
        {
            Type = type;
        }

        public abstract void Publish(in TBase message);
        public abstract void Dispose();
    }

    internal class Publisher<TBase, T> : Publisher<TBase>, IPublisher<TBase, T>
        where TBase : notnull, IMessage
        where T : TBase
    {
        private delegate void ProcessDelegate(in Guid messageId, in T message);

        private ProcessDelegate? _subscribers;

        public Publisher(IBaseSubscriber<TBase, T> subscriber) : base(typeof(T))
        {
            _subscribers = subscriber.Process;
        }

        public override void Publish(in TBase message)
        {
            if (message is T casted)
            {
                this.Publish(casted);
                return;
            }

            throw new ArgumentException($"{nameof(Publisher<TBase, T>)}::{nameof(Publish)} - Unable to assign {typeof(T).Name} from {message.GetType().Name}", nameof(message));
        }

        public void Publish(in T message)
        {
            Guid messageId = Guid.NewGuid();
            _subscribers?.Invoke(in messageId, in message);
        }

        public void Subscribe(IBaseSubscriber<TBase, T> subscriber)
        {
            _subscribers += subscriber.Process;
        }

        public void Unsubscribe(IBaseSubscriber<TBase, T> subscriber)
        {
            _subscribers -= subscriber.Process;
        }

        public override void Dispose()
        {
            _subscribers = null;
        }
    }
}
