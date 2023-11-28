using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBus : IBroker<IMessage>
    {
        Guid Id { get; }

        void Enqueue(in IMessage message);

        void Flush();
    }
}
