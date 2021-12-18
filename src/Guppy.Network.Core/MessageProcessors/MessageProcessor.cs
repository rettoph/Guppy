using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.MessageProcessors
{
    public abstract class MessageProcessor : IDisposable
    {
        public abstract void Process(Message message);

        public abstract void Dispose();
    }

    public abstract class MessageProcessor<TData> : MessageProcessor
        where TData : class, IData
    {
        public override void Process(Message message)
        {
            if(message.Data is TData data)
            {
                this.Process(data, message);
                return;
            }

            throw new ArgumentException(nameof(message));
        }

        protected abstract void Process(TData data, Message message);
    }
}
