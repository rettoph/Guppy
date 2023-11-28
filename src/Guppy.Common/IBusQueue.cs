using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBusQueue<TBase>
        where TBase : IMessage
    {
        int Id { get; }
        void Enqueue(in TBase message);
        void Flush();
    }
}
