using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Messaging
{
    public interface ISubscriber<in TMessage> : IBaseSubscriber<IMessage, TMessage>
        where TMessage : IMessage
    {

    }
}
