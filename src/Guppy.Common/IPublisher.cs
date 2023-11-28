using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IPublisher<TBase> : IDisposable
        where TBase : notnull, IMessage
    {
        Type Type { get; }

        void Publish(in TBase message);
    }

    public interface IPublisher<TBase, T> : IPublisher<TBase>
        where TBase : notnull, IMessage
        where T : TBase
    {
        void Publish(in T message);

        void Subscribe(IBaseSubscriber<TBase, T> subscriber);
        void Unsubscribe(IBaseSubscriber<TBase, T> subscriber);
    }
}
