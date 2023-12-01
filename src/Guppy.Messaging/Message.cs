using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Messaging
{
    public class Message<T> : IMessage
    {
        public Type Type => typeof(T);
    }
}
