using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Messaging
{
    public interface IBaseSubscriber<TBase>
        where TBase : class, IMessage
    {
    }

    public interface IBaseSubscriber<TBase, in TMessage> : IBaseSubscriber<TBase>
        where TBase : class, IMessage
        where TMessage : TBase
    {
        void Process(in Guid messageId, TMessage message);
    }
}
