using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IPublisher<TMessage>
        where TMessage : notnull
    {
        Type Type { get; }

        void Publish(in TMessage message);
    }

    public interface IPublisher<T, TMessage> : IPublisher<TMessage>
        where TMessage : notnull
        where T : TMessage
    {
        void Publish(in T message);

        void Subscribe(ISubscriber<T> subscriber);
        void Unsubscribe(ISubscriber<T> subscriber);
    }
}
