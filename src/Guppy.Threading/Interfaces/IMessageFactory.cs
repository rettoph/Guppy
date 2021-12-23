using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Interfaces
{
    public interface IMessageFactory<TMessage>
        where TMessage : class, IMessage
    {
        TMessage Create();
    }
}
