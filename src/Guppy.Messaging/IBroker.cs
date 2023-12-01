using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Messaging
{
    public interface IBroker<TBase>
        where TBase : class, IMessage
    {
        public void Publish(in TBase message);

        void Subscribe(IBaseSubscriber<TBase> subscriber);

        void Unsubscribe(IBaseSubscriber<TBase> subscriber);
    }
}
