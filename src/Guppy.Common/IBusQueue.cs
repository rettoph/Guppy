using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBusQueue
    {
        int Id { get; }
        void Enqueue(in IMessage message);
        void Publish(in IMessage message);
        void Flush();
    }
}
