using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IPublisher<T>
        where T : notnull
    {
        Type Type { get; }

        void Publish(in T message);
    }

    public interface IPublisher<T, TBase> : IPublisher<TBase>
        where TBase : notnull
        where T : TBase
    {
        void Publish(in T message);

        void Subscribe(ISubscriber<T> subscriber);
        void Unsubscribe(ISubscriber<T> subscriber);
    }
}
