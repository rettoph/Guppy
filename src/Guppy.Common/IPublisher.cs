using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IPublisher : IDisposable
    {
        Type Type { get; }

        void Publish(in IMessage message);
    }

    public interface IPublisher<T> : IPublisher
        where T : notnull, IMessage
    {
        void Publish(in T message);

        void Subscribe(ISubscriber<T> subscriber);
        void Unsubscribe(ISubscriber<T> subscriber);
    }
}
