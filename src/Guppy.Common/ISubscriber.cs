using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface ISubscriber : IBaseSubscriber<IMessage>
    {

    }
    public interface ISubscriber<T> : ISubscriber, IBaseSubscriber<IMessage, T>
        where T : IMessage
    {
    }
}
