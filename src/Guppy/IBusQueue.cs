using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBusQueue<T>
        where T : notnull
    {
        int Id { get; }
        void Enqueue(Type type, T message);
        void Flush(IBroker<T> broker);
    }
}
