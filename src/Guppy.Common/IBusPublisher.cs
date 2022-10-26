using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBusPublisher<T> : IDisposable
        where T : notnull, IMessage
    {
        void Subscribe(IBus<T> bus);
        void Unsubscribe(IBus<T> bus);
    }

    public interface IBusPublisher : IBusPublisher<IMessage>
    { 
    
    }
}
