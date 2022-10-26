using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBusQueue<T>
        where T : notnull, IMessage
    {
        int Id { get; }
        void Enqueue(in T message);
        void Flush(IBroker<T> broker);
    }
}
